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

namespace GPStudio.Client
{
	/// <summary>
	/// This interface defines the required behavior of any code that wants
	/// to stay current with the activities of the batch processing manager.
	/// </summary>
	public interface IBatchClient : IComparable<IBatchClient>
	{
		/// <summary>
		/// Informs the client a new simulation was added to the queue
		/// </summary>
		/// <param name="ProcessID"></param>
		/// <param name="ProjectName"></param>
		/// <param name="ProfileName"></param>
		/// <param name="TimeAdded"></param>
		void AddProcess(object ProcessID,String ProjectName,String ProfileName,DateTime TimeAdded,DateTime TimeStarted,bool IsStarted);
		/// <summary>
		/// Informs the client a new simulation has started
		/// </summary>
		/// <param name="ProcessID"></param>
		/// <param name="TimeStarted"></param>
		/// <param name="MaxGenerations"></param>
		void ProcessStarted(object ProcessID, DateTime TimeStarted,int MaxGenerations);
		/// <summary>
		/// Informs the client the generation has completed
		/// </summary>
		/// <param name="Generation"></param>
		void ProcessUpdate(int Generation);
		/// <summary>
		/// Informs the client of any modeling status information from the modeler
		/// </summary>
		/// <param name="Message"></param>
		void ProcessStatus(string Message);
		/// <summary>
		/// Informs the client a simulation has completed, along with some statistical information
		/// regarding the result.
		/// </summary>
		/// <param name="ProcessID"></param>
		/// <param name="TimeComplete"></param>
		/// <param name="Canceled"></param>
		/// <param name="Fitness"></param>
		/// <param name="Hits"></param>
		/// <param name="Complexity"></param>
		void ProcessComplete(object ProcessID,DateTime TimeComplete,bool Canceled,double Fitness,int Hits,int Complexity);
	}
}
