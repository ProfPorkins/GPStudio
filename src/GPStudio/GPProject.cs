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
using System.Data.OleDb;
using System.Data;

namespace GPStudio.Client
{
	public class GPProject
	{
		private int m_dbCode=0;
		private String m_sName;
		private int m_DataTrainingID=0;
		private List<int> m_DataValidation;
		private List<int> m_ModelProfiles;


		public GPProject(String sName)
		{
			m_dbCode = 0;
			m_DataTrainingID = 0;
			m_DataValidation=new List<int>();
			Name = sName;

			m_ModelProfiles=new List<int>();
		}

		public int DBCode
		{
			get { return m_dbCode; }
			set { m_dbCode = value; }
		}

		public String Name
		{
			get { return m_sName; }
			set { m_sName = value; }
		}

		public int DataTrainingID
		{
			get { return m_DataTrainingID; }
			set { m_DataTrainingID = value; }
		}

		public List<int> DataValidation
		{
			get { return m_DataValidation; }
		}

		public List<int> ModelProfiles
		{
			get { return m_ModelProfiles; }
		}

		//
		// Load the specified project
		public bool Load(int ProjectID)
		{
			this.DBCode = ProjectID;
			//
			// Load the primary settings - tblProject
			Load_tblProject(ProjectID);

			Load_tblValidationFiles(ProjectID);

			//
			// Load the profiles for this project - tblProjectProfiles
			Load_tblProjectProfiles(ProjectID);

			return true;
		}

		//
		// Grab the main project settings
		private bool Load_tblProject(int ProjectID)
		{
			OleDbConnection con = GPDatabaseUtils.Connect();
			try
			{
				OleDbDataAdapter daTable = new OleDbDataAdapter("SELECT Name,Description,TrainingFileID FROM tblProject WHERE DBCode = " + ProjectID, con);
				DataSet dSet = new DataSet();
				daTable.Fill(dSet);

				//
				// "In Theory" there should be only one row - guaranteed :)
				foreach (DataRow row in dSet.Tables[0].Rows)
				{
					this.Name = row.ItemArray.GetValue(0).ToString();
					this.DataTrainingID = Convert.ToInt32(row.ItemArray.GetValue(2).ToString());
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				con.Close();
			}

			return true;
		}

		//
		// Grab the validation files associated with the project
		private bool Load_tblValidationFiles(int ProjectID)
		{
			OleDbConnection con = GPDatabaseUtils.Connect();

			OleDbDataAdapter daTable = new OleDbDataAdapter("SELECT ModelingFileID FROM tblValidationFiles WHERE ProjectID = " + ProjectID, con);
			DataSet dSet = new DataSet();
			daTable.Fill(dSet);

			//
			// Remove any existing files
			m_DataValidation.Clear();

			//
			// Each row is a single profile
			foreach (DataRow row in dSet.Tables[0].Rows)
			{
				this.DataValidation.Add(Convert.ToInt32(row.ItemArray.GetValue(0).ToString()));
			}

			con.Close();

			return true;
		}

		//
		// Load the list of profiles for this project
		private bool Load_tblProjectProfiles(int ProjectID)
		{
			OleDbConnection con = GPDatabaseUtils.Connect();

			OleDbDataAdapter daTable = new OleDbDataAdapter("SELECT ModelProfileID FROM tblProjectProfiles WHERE ProjectID = " + ProjectID, con);
			DataSet dSet = new DataSet();
			daTable.Fill(dSet);

			//
			// Remove any existing profiles
			m_ModelProfiles.Clear();

			//
			// Each row is a single profile
			foreach (DataRow row in dSet.Tables[0].Rows)
			{
				this.ModelProfiles.Add(Convert.ToInt32(row.ItemArray.GetValue(0).ToString()));
			}

			con.Close();

			return true;
		}

		/// <summary>
		/// Save this project to the database.  If the project already exists,
		/// only perform an update.  Need to be careful not to delete any existing
		/// results.
		/// </summary>
		/// <returns></returns>
		public bool Save()
		{
			//
			// Determine whether we are updating or saving
			if (m_dbCode == 0)
			{
				//
				// If a brand new project, create the entry.
				CreateProjectEntry();
			}

			//
			// Update the settings
			UpdateProject();

			return true;
		}

		/// <summary>
		/// Removes the selected profile from the database
		/// </summary>
		/// <param name="ProfileID">DBCode of the profile to remove</param>
		/// <returns>True/False upon success or failure</returns>
		public bool DeleteProfile(int ProfileID)
		{
			//
			// Open the database connection
			OleDbConnection con = GPDatabaseUtils.Connect();

			String sSQL = "DELETE FROM tblProjectProfiles WHERE ProjectID = " + this.DBCode;
			sSQL += " AND ModelProfileID = " + ProfileID;

			OleDbCommand cmd = new OleDbCommand("Delete Profile", con);
			cmd.CommandText = sSQL;
			cmd.CommandType = CommandType.Text;
			//
			// Execute the command
			try
			{
				cmd.ExecuteNonQuery();
				//
				// Remove the entry from the list of profiles
				m_ModelProfiles.Remove(ProfileID);
			}
			catch (OleDbException)
			{
				return false;
			}

			con.Close();

			return true;
		}

		/// <summary>
		/// Update all the settings for this project
		/// </summary>
		private void UpdateProject()
		{
			//
			// Update the tblProject
			Update_tblProject();

			//
			// Update the selected validation files
			Update_tblValidationFiles();

			//
			// Update the selected profiles
			Update_tblProjectProfiles();
		}

		/// <summary>
		/// Update the project settings to tblProject
		/// </summary>
		private void Update_tblProject()
		{
			OleDbConnection con = GPDatabaseUtils.Connect();

			OleDbCommand cmd = new OleDbCommand("UPDATE tblProject SET Name = '" + this.Name + "' WHERE DBCode = " + this.DBCode, con);
			cmd.ExecuteNonQuery();

			if (this.DataTrainingID != 0)
			{
				cmd = new OleDbCommand("UPDATE tblProject SET TrainingFileID = " + this.DataTrainingID + " WHERE DBCode = " + this.DBCode, con);
				cmd.ExecuteNonQuery();
			}

			con.Close();
		}

		//
		// Update the list of validation files.  Just wipe out whatever was there
		// previously and rewrite the values.
		private bool Update_tblValidationFiles()
		{
			//
			// Open the database connection
			OleDbConnection con = GPDatabaseUtils.Connect();

			String sSQL = "DELETE FROM tblValidationFiles WHERE ProjectID = " + this.DBCode;

			OleDbCommand cmd = new OleDbCommand("Delete Files", con);
			cmd.CommandText = sSQL;
			cmd.CommandType = CommandType.Text;
			//
			// Execute the command
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (OleDbException)
			{
				return false;
			}

			//
			// Add everything back into the database
			foreach (int FileID in this.DataValidation)
			{
				sSQL = "INSERT INTO tblValidationFiles(ProjectID,ModelingFileID) ";
				sSQL += "VALUES ('" + this.DBCode + "',";
				sSQL += "'" + FileID + "')";

				cmd = new OleDbCommand("Add Validation File", con);
				cmd.CommandText = sSQL;
				cmd.CommandType = CommandType.Text;

				//
				// Execute the command
				try
				{
					cmd.ExecuteNonQuery();
				}
				catch (OleDbException ex)
				{
					string sError = ex.Message.ToString();
					return false;
				}
			}

			con.Close();

			return true;
		}

		//
		// Handles updating the list of profiles chosen by the user for this
		// project.  Two things have to happen here...
		//  1.  New profiles have to be added
		//  3.  Existing profiles have to be left the hell alone!
		private bool Update_tblProjectProfiles()
		{
			//
			// Go through each profile and see if it already exists, if not
			// then add it...simple enough.
			foreach (int ProfileID in m_ModelProfiles)
			{
				AddProjectProfile(ProfileID);
			}

			return true;
		}

		//
		// Handles the details of seeing if a profile is already defined for
		// the project, if not, it is added.
		private void AddProjectProfile(int ProfileID)
		{
			OleDbConnection con = GPDatabaseUtils.Connect();

			//
			// Check to see if the profile is already defined
			OleDbCommand cmd = new OleDbCommand("Find Profile",con);
			cmd.CommandText = "SELECT DBCode FROM tblProjectProfiles WHERE ModelProfileID = "+ProfileID+" AND ProjectID = "+this.DBCode;
			cmd.CommandType = CommandType.Text;

			OleDbDataReader reader=cmd.ExecuteReader();
			if (!reader.HasRows)
			{
				cmd.Dispose();
				//
				// Add the profile
				String sSQL = "INSERT INTO tblProjectProfiles(ProjectID,ModelProfileID) ";
				sSQL += "VALUES ('" + this.DBCode + "',";
				sSQL += "'" + ProfileID + "')";

				cmd = new OleDbCommand("Add Profile", con);
				cmd.CommandText = sSQL;
				cmd.CommandType = CommandType.Text;
				cmd.ExecuteNonQuery();
			}

			con.Close();
		}


		//
		// Create an initial database entry for the project
		private bool CreateProjectEntry()
		{
			OleDbConnection con = GPDatabaseUtils.Connect();

			//
			// Build the command to add the entry to the database
			String sSQL="INSERT INTO tblProject(Name) ";
			sSQL += "VALUES ('" + this.Name + "')";

			OleDbCommand cmd = new OleDbCommand("Add Project", con);
			cmd.CommandText = sSQL;
			cmd.CommandType = CommandType.Text;

			//
			// Execute the command
			try
			{
				cmd.ExecuteNonQuery();
				//
				// Grab the DBCode generated for this file
				cmd.CommandText = "SELECT @@IDENTITY";
				OleDbDataReader reader = cmd.ExecuteReader();
				reader.Read();
				DBCode = Convert.ToInt32(reader[0].ToString()); ;
				reader.Close();
			}
			catch (OleDbException ex)
			{
				string sError = ex.Message.ToString();
				return false;
			}

			con.Close();

		return true;
		}

		//
		// A static method available to delete a project entry
		public static bool DeleteProject(int ProjectID)
		{
			//
			// Open the database connection
			OleDbConnection con = GPDatabaseUtils.Connect();

			String sSQL = "DELETE FROM tblProject WHERE DBCode = " + ProjectID;

			OleDbCommand cmd = new OleDbCommand("Delete Project", con);
			cmd.CommandText = sSQL;
			cmd.CommandType = CommandType.Text;
			//
			// Execute the command
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (OleDbException)
			{
				return false;
			}

			con.Close();

			return true;
		}
	}
}
