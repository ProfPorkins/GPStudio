/*
Copyright (c) 2018 James Dean Mathias

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;	// For MethodInvoker

namespace GPStudio.Client
{
	/// <summary>
	/// This class manages the batch processing of models, as requested by the user.
	/// Batch processing can take place without the Batch Processing form being active.
	/// However, if it active, this class provides the ability to report results so
	/// a visual status of the modeling can be maintained.
	/// </summary>
	public class BatchProcessingManager
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public BatchProcessingManager()
		{
			//
			// Get the worker thread created
			InitWorkerThread();
		}

		/// <summary>
		/// Batch process queue entry
		/// </summary>
		private class BatchProcess : IComparable<BatchProcess>
		{
			public BatchProcess(GPProjectProfile Profile, int TrainingDataID, int ProjectID)
			{
				this.Profile = Profile;
				this.TrainingDataID = TrainingDataID;
				this.ProjectID = ProjectID;

				this.TimeAdded = DateTime.Now;
				this.IsStarted = false;
				this.Canceled = false;
			}

			public GPProjectProfile Profile;
			public int TrainingDataID;
			public int ProjectID;
			public DateTime TimeAdded;
			public DateTime TimeStarted;
			public bool IsStarted;
			public DateTime TimeCompleted;
			public bool Canceled;

			//
			// Modeling stats
			public double Fitness;
			public int Hits;
			public int Complexity;

			#region IComparable<BatchProcess> Members

			public int CompareTo(BatchProcess other)
			{
				if (other == this)
				{
					return 0;
				}

				return 1;
			}

			#endregion
		}

		/// <summary>
		/// Creates the worker thread that manages the batch processing of models.
		/// </summary>
		private void InitWorkerThread()
		{
			//
			// We create a queue of commands, because many different client commands may come
			// in durring processing and we need to ensure all commands are captured and executed.
			m_CommandQueue = new Queue<BatchProcessingManager.Command>();
			//
			// Create a wait handle that puts the thread in a suspended state
			// waiting for a command event to come through.
			wh_CommandEvent = new EventWaitHandle(false, EventResetMode.ManualReset);

			m_Thread = new Thread(new ParameterizedThreadStart(BatchThreadStart));
			m_Thread.Start(this);
		}

		private Thread m_Thread;
		private EventWaitHandle wh_CommandEvent;
		private Queue<BatchProcessingManager.Command> m_CommandQueue;

		/// <summary>
		/// Private thread that manages the batch modeling
		/// </summary>
		/// <param name="arg">Reference to the object the thread belongs to</param>
		private void BatchThreadStart(object arg)
		{
			BatchProcessingManager ThreadObj = (BatchProcessingManager)arg;
			bool Done = false;
			while (!Done)
			{
				wh_CommandEvent.WaitOne();		// Wait for a command event
				wh_CommandEvent.Reset();		// Immediately reset the event

				//
				// Do something based upon the oldest command in the queue - After every non-default command, remember
				// to call the default command so that the next batch process willl get executed.
				switch (m_CommandQueue.Dequeue())
				{
					case Command.RegisterClient:
						m_Clients.Add(m_EventClient.Peek(), m_EventClient.Peek());
						m_EventClient.Dequeue();
						m_CommandQueue.Enqueue(Command.Default);
						wh_CommandEvent.Set();
						break;
					case Command.UnregisterClient:
						m_Clients.Remove(m_EventClient.Dequeue());
						m_CommandQueue.Enqueue(Command.Default);
						wh_CommandEvent.Set();
						break;
					case Command.ExecSimulation:
						if (m_Queue.Count > 0)
						{
							m_Queue[0].TimeStarted = DateTime.Now;
							m_Queue[0].IsStarted = true;
							BroadcastModelStarted(m_Queue[0]);
							//
							// Model the first one in the queue
							BatchModel(m_Queue[0]);
							//
							// Report to clients the model is now finished
							BroadcastModelComplete(m_Queue[0]);
							//
							// Simulation is done, remove the entry
							m_Queue.RemoveAt(0);
							//
							// Check for more commands
							m_CommandQueue.Enqueue(Command.Default);
							wh_CommandEvent.Set();
						}
						break;
					case Command.Terminate:
						m_CommandQueue.Clear();
						m_Clients.Clear();
						Done = true;
						break;
					default:
						//
						// If there are any pending commands, deal with them first
						if (m_CommandQueue.Count > 0)
						{
							wh_CommandEvent.Set();
						}
						else if (m_Queue.Count > 0)	// Clear any pending batch processes
						{
							m_CommandQueue.Enqueue(Command.ExecSimulation);
							wh_CommandEvent.Set();
						}
						break;
				}
			}
		}

		/// <summary>
		/// Manages the modeling for a batch process.
		/// </summary>
		/// <param name="Process"></param>
		/// <returns>true if model was created, false otherwise</returns>
		private bool BatchModel(BatchProcess Process)
		{
#if GPLOG
			GPLog.ReportLine("Batch Processing: Requesting a program.",true);
#endif
			//
			// Create the modeler object - the parameter to this thread is
			// the model profile.
			m_Modeler = new GPModeler(
				Process.Profile,
				Process.TrainingDataID,
				null,	//new GPModeler.DEL_ValidatedServer(AddValidatedServer),
				new GPModeler.DEL_ReportStatus(ReceiveStatus),
				new GPModeler.DEL_ReportFitness(ReceiveFitness),
				null);	//new GPModeler.DEL_GenerationComplete(GenerationComplete));
				
			//
			// Make an asynchronous call that gets the modeling started
			MethodInvoker miModeler = new MethodInvoker(m_Modeler.RequestProgram);
			IAsyncResult ar = miModeler.BeginInvoke(null, null);

			//
			// Wait for the modeling to complete
			miModeler.EndInvoke(ar);

			//
			// Record the time of completion
			Process.TimeCompleted = DateTime.Now;

			//
			// Check to see if we were canceled
			if (m_CancelSimulation)
			{
#if GPLOG
				GPLog.ReportLine("Batch Processing: Simulation Canceled",true);
#endif
				Process.Canceled = true;
				//
				// Reset the cancel flag
				m_CancelSimulation = false;
				return false;
			}

#if GPLOG
			GPLog.ReportLine("Batch Processing: Program request complete.",true);
#endif

			//
			// Save the best program to the database
			int ProgramID = 0;	// A dummy variable
			m_Modeler.SaveBestToDB(Process.ProjectID, ref ProgramID);

			return true;
		}
		private GPModeler m_Modeler;

		/// <summary>
		/// Method that receives reports of fitness from the modeler
		/// </summary>
		/// <param name="Server"></param>
		/// <param name="Generation"></param>
		/// <param name="Stats"></param>
		private void ReceiveFitness(int Server, int Generation, ModelingResults.ServerData Stats)
		{
			//
			// Update the process with the current modeling stats
			m_Queue[0].Fitness = Stats.BestFitness;
			m_Queue[0].Hits = Stats.BestHits;
			m_Queue[0].Complexity = Stats.BestComplexity;

			//
			// TODO:  HACK, HACK, HACK and I know it!  I'm not proud of it, but I'm doing
			// it anyway.  I'm having trouble synchronizing the registration of new clients
			// while batch processing is taking place, so this is my hack to deal with it.
			UpdatePendingRegistrations();

			//
			// Broadcast this information
			foreach (KeyValuePair<IBatchClient, IBatchClient> Client in m_Clients)
			{
				Client.Value.ProcessUpdate(Generation);
			}
		}

		private void UpdatePendingRegistrations()
		{
			while (m_CommandQueue.Count > 0 && 
				(m_CommandQueue.Peek() == Command.Default || m_CommandQueue.Peek() == Command.RegisterClient || m_CommandQueue.Peek() == Command.UnregisterClient))
			{
				switch (m_CommandQueue.Dequeue())
				{
					case Command.RegisterClient:
						m_Clients.Add(m_EventClient.Peek(), m_EventClient.Peek());
						m_EventClient.Dequeue();
						break;
					case Command.UnregisterClient:
						m_Clients.Remove(m_EventClient.Dequeue());
						break;
				}
			}
		}

		/// <summary>
		/// Method that receives reports of the modeling status from the modeler
		/// </summary>
		/// <param name="Message"></param>
		private void ReceiveStatus(string Message)
		{
			//
			// Broadcast the status message
			foreach (KeyValuePair<IBatchClient, IBatchClient> Client in m_Clients)
			{
				Client.Value.ProcessStatus(Message);
			}
		}

		#region Client Reporting

		/// <summary>
		/// Sends notification to all clients a batch process has just started, indicating which one it is.
		/// </summary>
		/// <param name="Process"></param>
		private void BroadcastModelStarted(BatchProcess Process)
		{
			foreach (KeyValuePair<IBatchClient, IBatchClient> Client in m_Clients)
			{
				Client.Value.ProcessStarted(Process, Process.TimeStarted,Process.Profile.m_maxNumber);
			}
		}

		/// <summary>
		/// Sends notification to any clients a batch process has finished modeling
		/// </summary>
		/// <param name="Process"></param>
		private void BroadcastModelComplete(BatchProcess Process)
		{
			UpdatePendingRegistrations();
			foreach (KeyValuePair<IBatchClient, IBatchClient> Client in m_Clients)
			{
				Client.Value.ProcessComplete(
					Process, 
					Process.TimeCompleted,
					Process.Canceled,
					Process.Fitness,
					Process.Hits,
					Process.Complexity);
			}
		}

		#endregion

		#region Public Interface

		/// <summary>
		/// Allows code to register itself to receive notifications of batch processing events
		/// </summary>
		/// <param name="Client">Reference to the IBatchClient interface</param>
		public void RegisterClient(IBatchClient Client)
		{
			//
			// Upon new registration, let the client know about all pending batch processes.
			foreach (BatchProcess Process in m_Queue)
			{
				Client.AddProcess(
					Process,
					GPDatabaseUtils.FieldValue(Process.ProjectID,"tblProject","Name"),
					Process.Profile.Name,
					Process.TimeAdded,
					Process.TimeStarted,
					Process.IsStarted);
			}

			//
			// Indicate to the thread a new client has been added
			m_EventClient.Enqueue(Client);
			m_CommandQueue.Enqueue(Command.RegisterClient);
			wh_CommandEvent.Set();
		}
		private SortedDictionary<IBatchClient, IBatchClient> m_Clients = new SortedDictionary<IBatchClient, IBatchClient>();
		private Queue<IBatchClient> m_EventClient = new Queue<IBatchClient>();

		/// <summary>
		/// Allows a client to unregister itself from event notification.
		/// </summary>
		/// <param name="Client">Reference to the IBatchClient interface to be removed</param>
		public void UnregisterClient(IBatchClient Client)
		{
			//
			// Indicate to the thread a client should be removed
			m_EventClient.Enqueue(Client);
			m_CommandQueue.Enqueue(Command.UnregisterClient);
			wh_CommandEvent.Set();
		}

		/// <summary>
		/// These are the different commands the batch processing manager can send.  Right now
		/// it's pretty simple, execute and simulation and terminate the active simulation.
		/// </summary>
		private enum Command
		{
			Default,			// Have the thread check to see if there are any more pending processes
			ExecSimulation,
			Terminate,
			RegisterClient,
			UnregisterClient
		}

		/// <summary>
		/// Add a new simulation to be performed.
		/// </summary>
		public void AddSimulation(GPProjectProfile Profile,int TrainingDataID,int ProjectID)
		{
			BatchProcess NewProcess = new BatchProcess(Profile, TrainingDataID,ProjectID);
			m_Queue.Add(NewProcess);

			//
			// Inform all clients a new process was just added
			foreach (KeyValuePair<IBatchClient,IBatchClient> Client in m_Clients)
			{
				Client.Value.AddProcess(
					NewProcess,  
					GPDatabaseUtils.FieldValue(NewProcess.ProjectID, "tblProject", "Name"),
					NewProcess.Profile.Name,
					NewProcess.TimeAdded,
					NewProcess.TimeStarted,
					NewProcess.IsStarted);
			}

			//
			// Indicate to the thread a new model has been requested.  This is done by sending
			// a "Default" command.  What the effectively does is to make execing of simulations
			// the lowest priority event.  We want register client events to take priority
			// over exec simulations, so they start getting updated as soon as possible.
			m_CommandQueue.Enqueue(Command.Default);
			wh_CommandEvent.Set();
		}

		/// <summary>
		/// Container to hold the current list of pending batch processes.  Using a List instead
		/// of a Queue, because we need to access members that are in places other than the
		/// front of the queue.
		/// </summary>
		private List<BatchProcess> m_Queue = new List<BatchProcess>();

		/// <summary>
		/// Instructs the manager to cancel indicated batch process.  If the
		/// process is active, it is canceled.  If it is somewhere else in
		/// the queue, it is marked as canceled.
		/// </summary>
		public void CancelSimulation(object ProcessID)
		{
			//
			// If the active process, tell the modeler to quit
			if (m_Queue[0] == (BatchProcess)ProcessID)
			{
				m_CancelSimulation = true;
				m_Modeler.Abort();
			}
			else
			{
				m_Queue.Remove((BatchProcess)ProcessID);
			}
		}
		private bool m_CancelSimulation = false;

		/// <summary>
		/// Get everything shut down
		/// </summary>
		public void CancelAllSimulations()
		{
			//
			// Ensure we have something to cancel
			if (m_Queue.Count == 0) return;

			//
			// Start by removing processes from the back of the queue, this prevents us from terminating
			// the active simulation, causing the next one to start, then terminating another active
			// simulation and etc.  If we start at the back, we only ever terminate on active simulation,
			// making it much more resource and time friendly.
			while (m_Queue.Count > 1)
			{
				m_Queue.RemoveAt(m_Queue.Count - 1);
			}
			//
			// Cancel the final simulation
			this.CancelSimulation(m_Queue[0]);
		}

		/// <summary>
		/// Swaps these two processes in the pending queue
		/// </summary>
		/// <param name="ProcessID1"></param>
		/// <param name="ProcessID2"></param>
		public void SwapProcesses(object ProcessID1, object ProcessID2)
		{
			//
			// Step 1 - Remove the second item
			m_Queue.Remove((BatchProcess)ProcessID2);
			//
			// Step 2 - Get the index of the first one, for the insert location
			int InsertIndex=m_Queue.IndexOf((BatchProcess)ProcessID1);
			//
			// Step 3 - Insert the second item before the first
			m_Queue.Insert(InsertIndex,(BatchProcess)ProcessID2);
		}

		/// <summary>
		/// Allows the batch processing to be suspended.  For example, to prevent
		/// new processes from being started while changes to the pending processes
		/// are made.
		/// TODO: I know the compiler is saying that .Suspend is deprecated, but for
		/// crying out loud, it is easy to use, so I'm using it!
		/// </summary>
		public void SuspendProcessing()
		{
			m_Thread.Suspend();
		}

		/// <summary>
		/// Resumes the thread that manages the batch processing.
		/// </summary>
		public void ResumeProcessing()
		{
			m_Thread.Resume();
		}

		/// <summary>
		/// Tell the command thread to terminate.  This should only be called when the program
		/// is shutting down, cleaning up the BatchProcessingManager in the process.
		/// </summary>
		public void Terminate()
		{
			//
			// Tell the thread to terminate
			m_CommandQueue.Enqueue(Command.Terminate);
			wh_CommandEvent.Set();
		}

		/// <summary>
		/// Property that allows anyone to find out if any simulations are currently active.
		/// </summary>
		public bool ActiveSimulation
		{
			get
			{
				if (m_Queue.Count > 0) return true;

				return false;
			}
		}

		#endregion
	}
}
