using System;
using System.Windows.Forms;
namespace FrontEnd
{
	/// <summary>
	/// Starts the app
	/// </summary>
	public class StartUp
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

	}
}
