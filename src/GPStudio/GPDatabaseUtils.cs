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
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using ADOX;
using GPStudio.Shared;

//
// A set of utilities that provide support for database communication.
namespace GPStudio.Client
{
	public class GPDatabaseUtils
	{
		/// <summary>
		/// Opens a connection to the database
		/// </summary>
		/// <returns></returns>
		public static OleDbConnection Connect()
		{
			try
			{
				//
				// Make the actual connection
				OleDbConnection con = new OleDbConnection(ConnectionString);
				con.Open();

				return con;
			}
			catch
			{
				throw new Exception("Invalid database file");
			}
		}

		/// <summary>
		/// Builds the connection string needed to create an ADO connection to the database
		/// </summary>
		private static String ConnectionString
		{
			get
			{
				Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

				String Connection = "Provider=" + config.AppSettings.Settings["DBProvider"].Value;
				Connection += "; Data Source=" + config.AppSettings.Settings["GPDatabase"].Value;

				return Connection;
			}
		}

		/// <summary>
		/// Property that checks the database to see if it is compatible
		/// with the current version of the software.
		/// </summary>
		public static bool IsDatabaseValid
		{
			get
			{
				try
				{
					String Description = "";
					using (OleDbConnection con = Connect())
					{
						String SQL = "SELECT Description FROM tblAdministration";
						OleDbDataAdapter daTable = new OleDbDataAdapter(SQL, con);
						DataSet dSet = new DataSet();
						daTable.Fill(dSet);

						//
						// "In Theory" there should be only one row - guaranteed :)
						Description = dSet.Tables[0].Rows[0].ItemArray.GetValue(0).ToString();
					}
					//
					// Check to see if we need to upgrade this database
					if (Description.Trim() == GPEnums.DATABASE_UPGRADEVERSION)
					{
						return UpgradeDatabase();
					}

					if (Description.Trim() == GPEnums.DATABASE_VERSION)
					{
						return true;
					}
				}
				catch (Exception)
				{
					return false;
				}

				return false;
			}
		}

		/// <summary>
		/// Upgrades a Version 1.0 database to Version 1.1.  This upgrade is the
		/// addition of the field "TerminalParameters" to the table tblFunctionSet.
		/// </summary>
		/// <returns></returns>
		private static bool UpgradeDatabase()
		{
			try
			{
				ADODB.Connection con = new ADODB.Connection();
				con.Open(ConnectionString, "", "", 0);

				Catalog dbCatalog = new Catalog();
				dbCatalog.ActiveConnection = con;

				Table dbTable = dbCatalog.Tables["tblFunctionSet"];

				dbTable.Columns.Append("TerminalParameters", ADOX.DataTypeEnum.adBoolean, 1);
				dbTable.Columns[dbTable.Columns.Count - 1].Properties["Default"].Value = "False";

				con.Close();
			}
			catch (Exception)
			{
				MessageBox.Show("Failed to correctly upgrade the database");
				return false;
			}

			//
			// If the upgrade succeeded, update the version value in the admin table
			return UpdateField(1, "tblAdministration", "Description", GPEnums.DATABASE_VERSION);
		}

		/// <summary>
		/// Verifies the selected database can be used
		/// </summary>
		/// <returns>True/False upon success or failure</returns>
		public static bool ValidateDatabase(String DatabaseFile)
		{
			//
			// Temporarily update the applicaton config
			Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
			String DatabaseCurrent = config.AppSettings.Settings["GPDatabase"].Value;
			config.AppSettings.Settings["GPDatabase"].Value = DatabaseFile;
			config.Save();

			bool Valid = GPDatabaseUtils.IsDatabaseValid;

			//
			// return the original database name to the config
			config.AppSettings.Settings["GPDatabase"].Value = DatabaseCurrent;
			config.Save();

			return Valid;
		}

		/// <summary>
		/// Retrieves the string value for the given table and field.  This is
		/// such a commonly performed operation, I made a function to handle
		/// this.  It is not recommended to be used with high-frequency, because
		/// it opens a connection to the database each time it is used.
		/// </summary>
		/// <param name="DBCode">DBCode of the record in question</param>
		/// <param name="Table">Table to search</param>
		/// <param name="Field">Field to return</param>
		/// <returns></returns>
		public static String FieldValue(int DBCode,String Table, String Field)
		{
			try
			{
				using (OleDbConnection con = GPDatabaseUtils.Connect())
				{
					String SQL = "SELECT " + Field + " FROM " + Table;
					SQL += " WHERE DBCode = " + DBCode;

					OleDbCommand cmd = new OleDbCommand(SQL, con);
					OleDbDataReader rdr = cmd.ExecuteReader();

					rdr.Read();
					return rdr[Field].ToString();
				}
			}
			catch
			{
				return "<Undefined>";
			}

		}

		/// <summary>
		/// Updates an integer (ID) value for the specified table, field and record.
		/// This is a somewhat common task, so a function was made to do it.  This
		/// should not be used for a high-frequency situation.
		/// </summary>
		/// <param name="DBCode">DBCode of the record to update</param>
		/// <param name="Table">Table to update</param>
		/// <param name="Field">Field to update</param>
		/// <param name="Value">New integer value</param>
		/// <returns>True/False upon success or failure</returns>
		public static bool UpdateField(int DBCode, String Table, String Field, int Value)
		{
			using (OleDbConnection con = GPDatabaseUtils.Connect())
			{
				String SQL = "UPDATE " + Table + " SET " + Field + " = " + Value + " WHERE DBCode = " + DBCode;

				OleDbCommand cmd = new OleDbCommand(SQL, con);

				try
				{
					cmd.ExecuteNonQuery();
				}
				catch (OleDbException)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Updates an integer (ID) value for the specified table, field and record.
		/// This is a somewhat common task, so a function was made to do it.  This
		/// should not be used for a high-frequency situation.
		/// </summary>
		/// <param name="DBCode">DBCode of the record to update</param>
		/// <param name="Table">Table to update</param>
		/// <param name="Field">Field to update</param>
		/// <param name="Value">New string value</param>
		/// <returns>True/False upon success or failure</returns>
		public static bool UpdateField(int DBCode, String Table, String Field, String Value)
		{
			using (OleDbConnection con = GPDatabaseUtils.Connect())
			{
				String SQL = "UPDATE " + Table + " SET " + Field + " = \"" + Value + "\" WHERE DBCode = " + DBCode;

				OleDbCommand cmd = new OleDbCommand(SQL, con);

				try
				{
					cmd.ExecuteNonQuery();
				}
				catch (OleDbException)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Grab the function specs from the database
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="Arity"></param>
		/// <param name="Code"></param>
		/// <param name="LanguageID"></param>
		/// <returns></returns>
		public static bool LoadFunctionFromDB(String Name, ref short Arity, ref bool TerminalParameters, ref String Code, int LanguageID)
		{
			OleDbConnection con = GPDatabaseUtils.Connect();

			try
			{
				String SQL = "SELECT Arity,TerminalParameters,Code FROM tblFunctionSet WHERE Name = '" + Name + "' AND FunctionLanguageID = " + LanguageID;
				OleDbDataAdapter daFiles = new OleDbDataAdapter(SQL, con);
				DataSet dSet = new DataSet();
				daFiles.Fill(dSet);

				//
				// There better be a single row! :)
				Arity = Convert.ToInt16(dSet.Tables[0].Rows[0].ItemArray.GetValue(0).ToString());
				TerminalParameters = Convert.ToBoolean(dSet.Tables[0].Rows[0].ItemArray.GetValue(1).ToString());
				Code = dSet.Tables[0].Rows[0].ItemArray.GetValue(2).ToString();
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				con.Close();
			}

			return true;
		}
	}
}
