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

namespace GPStudio.Shared
{
	///
	/// <summary> Clearinghouse for the various built in enumerations for the GP
	/// system.
	/// </summary>
	public sealed class GPEnums
	{
		/// <summary>
		/// String name of the application.  Seems like this should be in a resource
		/// file and pulled out from there.
		/// </summary>
		public const string APPLICATON_NAME = "GP Studio 3.00 - Distributed";
		/// <summary>
		/// Defines the current GPServer version, used by the Validate method on
		/// the server side and by clients to ensure they are using the correct
		/// server version.
		/// </summary>
		public const int SERVER_VERSION = 7;

		/// <summary>
		/// Defines the current MS Access database version.  This is used to validate
		/// the correct database structure is used so that the program can run
		/// without error against the database.
		/// </summary>
		public const string DATABASE_VERSION = "Version 1.1";

		/// <summary>
		/// All Version 1.0 databases require a small modification to the tblFunctionSet table
		/// </summary>
		public const string DATABASE_UPGRADEVERSION = "Version 1.0";

		/// <summary>
		/// Visible SOAP name for the remote object
		/// </summary>
		public const string SERVER_SOAPNAME = "GPServer.soap";

		/// <summary>
		/// As if the acronyms TCP and HTTP would ever change, it somehow
		/// makes me feel better to define these as constants anyway.
		/// </summary>
		public const String CHANNEL_TCP = "TCP";
		public const String CHANNEL_HTTP = "HTTP";

		/// <summary>
		/// How long between checks to see if a remote object should be automatically
		/// destroyed, if the sponsor is no longer available.
		/// </summary>
		public const double REMOTING_RENEWAL_MINUTES = 60;	// 1 hour lifetime
		public const double REMOTING_TIMEOUT_MINUTES = 2;

		/// <summary>
		/// I'm not proud of this, it should be an application/modeling parameter,
		/// but at least it is defined as a constant.
		/// </summary>
		public const double RESULTS_TOLERANCE = 0.000001;

		/// <summary>
		/// If a program size gets above this, we'll give it a bad fitness value
		/// so that it doesn't get used in future genetic operations.
		/// </summary>
		public const int PROGRAMSIZE_THRESHOLD = 25000;

		/// <summary>
		/// Formatting string for numeric display
		/// </summary>
		public const String NUMERIC_DISPLAY_FORMAT = "#,0.######";

		/// <summary>
		/// Population generation methods
		/// </summary>
		public enum PopulationInit
		{
			Full,
			Grow,
			Ramped,
		}	

		/// <summary>
		/// Different terminal types
		/// </summary>
		public enum TerminalType
		{
			RandomDouble,
			RandomInteger,
			UserDefined,
			ADFParameter,
			ADLParameter,
			ADRParameter
		}

		/// <summary>
		/// Program tree growth methods
		/// </summary>
		public enum TreeBuild
		{
			Undef,
			Full,
			Grow
		}
		
		/// <summary>
		/// Population reproduction methods
		/// </summary>
		public enum Reproduction
		{
			Tournament,
			OverSelection
		}

		/// <summary>
		/// Modeling types
		/// </summary>
		public enum ModelType
		{
			Regression,
			TimeSeries
		}

		/// <summary>
		/// Distributed server topology organizations
		/// 
		/// Ring: Each server sends programs only to its two connected neighbors
		/// Star: Each server sends programs to every % of the other servers in the network
		/// </summary>
		public enum DistributedTopology
		{
			Ring,
			Star
		}

		//
		// Supported languages
		public const string LANGUAGE_C = "C";
		public const string LANGUAGE_CPP = "C++";
		public const string LANGUAGE_CSHARP = "C#";
		public const string LANGUAGE_VB = "Visual Basic";
		public const string LANGUAGE_VBNET = "Visual Basic.NET";
		public const string LANGUAGE_JAVA = "Java";
		public const string LANGUAGE_FORTRAN = "Fortran";
		public const string LANGUAGE_XML = "XML";

		//
		// Let's define the Tag values for each of the languages.  "In Theory" these
		// are absolutely set in stone.
		public const int LANGUAGEID_C = 1;
		public const int LANGUAGEID_CPP = 2;
		public const int LANGUAGEID_CSHARP = 3;
		public const int LANGUAGEID_VB = 4;
		public const int LANGUAGEID_VBNET = 5;
		public const int LANGUAGEID_JAVA = 6;
		public const int LANGUAGEID_FORTRAN = 7;
	}
}
