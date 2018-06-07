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
using System.Runtime.Remoting.Lifetime;
using GPStudio.Shared;

namespace GPStudio.Client
{
	public class GPServerClientSponsor : MarshalByRefObject, ISponsor
	{
		/// <summary>
		/// This property was orignally put here to help in debugging which sponsor was being
		/// called.  I've left it in, in case future debugging is needed with it.
		/// </summary>
		private string m_Name;

		public GPServerClientSponsor(string Name)
		{
			m_Name = Name;
			Renew = true;
		}

		/// <summary>
		/// Specifies the renewal time for the remote object
		/// </summary>
		/// <param name="LeaseInfo"></param>
		/// <returns></returns>
		public TimeSpan Renewal(ILease LeaseInfo)
		{
			//
			// Log the time the renewal was called
#if GPLOG
			GPLog.ReportLine("Sponsor Renewal - " + m_Name,true);
#endif

			if (Renew)
			{
				return TimeSpan.FromMinutes(GPEnums.REMOTING_RENEWAL_MINUTES);
			}

			return TimeSpan.Zero;
		}

		/// <summary>
		/// Property that indicates whether or not this sponsor should renew the remote object.
		/// </summary>
		public bool Renew
		{
			get { return m_Renew; }
			set { m_Renew = value; }
		}
		private bool m_Renew;
	}
}
