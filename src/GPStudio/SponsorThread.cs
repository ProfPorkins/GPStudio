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
using System.Threading;

namespace GPStudio.Client
{
	/// <summary>
	/// This class manages remote client sponsor objects in their own thread
	/// so they can be used asynchronously to the thread using the remote
	/// objects.
	/// </summary>
	public class SponsorThread
	{
		public SponsorThread()
		{
			//
			// Create a wait handle that puts the thread in a suspended state
			// waiting for a command event to come through.
			wh_Command = new EventWaitHandle(false, EventResetMode.ManualReset);

			m_Thread = new Thread(new ThreadStart(SponsorThreadStart));
			m_Thread.Start();
		}

		private Thread m_Thread;
		private EventWaitHandle wh_Command;
		private SponsorThread.Command m_Command;

		/// <summary>
		/// Private thread that creates client sponsor objects as needed.
		/// </summary>
		/// <param name="arg"></param>
		private void SponsorThreadStart()
		{
			bool Done = false;
			while (!Done)
			{
				wh_Command.WaitOne();	// Wait for a command to come through
				wh_Command.Reset();		// Immediately reset the event

				//
				// Do something based upon the command given
				switch (m_Command)
				{
					case Command.Terminate:
						Done = true;
						break;
					case Command.CreateSponsor:
						m_SponsorRef = new GPServerClientSponsor(m_SponsorName);
						wh_CommandComplete.Set();
						break;
				}
			}
		}

		#region Public Command Interface

		private enum Command
		{
			Terminate,
			CreateSponsor
		}

		/// <summary>
		/// Request the command thread to create a new sponsor object
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		public GPServerClientSponsor Create(String Name)
		{
			//
			// Create an event that will be set when the thread has the sponsor
			// object created.
			wh_CommandComplete = new EventWaitHandle(false, EventResetMode.ManualReset);

			//
			// Signal the thread to do something
			m_SponsorName = Name;
			m_Command = Command.CreateSponsor;
			wh_Command.Set();

			//
			// Wait for the operation to complete
			wh_CommandComplete.WaitOne();

			return m_SponsorRef;
		}

		private string m_SponsorName;
		private EventWaitHandle wh_CommandComplete;
		private GPServerClientSponsor m_SponsorRef;

		/// <summary>
		/// Tell the command thread to terminate
		/// </summary>
		public void Terminate()
		{
			//
			// Tell the thread to terminate
			m_Command = Command.Terminate;
			wh_Command.Set();
		}

		#endregion
	}
}
