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
using System.IO;
using System.Data.OleDb;
using System.Data;
using GPStudio.Shared;
//
// This class represents a single Training set used for a modeling session.
namespace GPStudio.Client
{
	public class GPModelingData
	{
		/// <summary>
		/// Default constructor, create the contained GPTrainingData object
		/// </summary>
		public GPModelingData()
		{
			//
			// Create the contained GPTrainingData object
			m_Training = new GPTrainingData();
		}

		private GPTrainingData m_Training;
		/// <summary>
		/// This class uses a containment model versus an inheritance model
		/// with the GPTrainingData class because of the serialization needs
		/// for the remoting...it's too much work and not needed work to deal
		/// with delegates and remoting for this class
		/// </summary>
		public GPTrainingData Training
		{
			get { return m_Training; }
		}

		/// <summary>
		/// Well, the GPServer doesn't give a rat's ass about time series data
		/// formats...it just wants inputs and it makes a program.  The real reason
		/// for this is to keep the GPServer code simple and get rid of a lot
		/// of unneeded complexities.  So, this will properly transform a timeseries
		/// training data set into something usable by the GPServer.
		/// </summary>
		public GPTrainingData TrainingForModeling(int InputDimension,int PredictionDistance)
		{
			if (TimeSeries)
			{
				return ConvertTSToModeling(InputDimension,PredictionDistance);
			}
			else
			{
				return m_Training;
			}
		}

		/// <summary>
		/// Converts a time series training data set into something that looks
		/// like a normal data set with some number of inputs.
		/// </summary>
		/// <returns>Read the summary section</returns>
		private GPTrainingData ConvertTSToModeling(int InputDimension,int PredictionDistance)
		{
			GPTrainingData TSTraining = new GPTrainingData();
			TSTraining.TimeSeriesSource = true;

			//
			// The number of rows is the number of rows in the Training
			// set minus the input dimension and the prediction distance.  We subtract this off
			// because we can't predict anything before the first 'n' input values.
			int Rows=Training.Rows-InputDimension-PredictionDistance+1;
			TSTraining.ConstructStorage(Rows, InputDimension, 1);

			//
			// Now, put together the rows of input data and the objective for each
			// of these rows.  
			for (int Row = 0; Row < Rows; Row++)
			{
				//
				// Set the inputs
				for (int Column = 0; Column < InputDimension; Column++)
				{
					TSTraining[Row, Column] = Training[Row + Column, 0];
				}

				//
				// Set the objective
				TSTraining.ObjectiveRow(Row)[0] = Training[Row +InputDimension+ PredictionDistance-1,0];
			}

			return TSTraining;
		}

		//
		// These are the properties and methods that wrap the contained GPTrainingData
		// object; these work as simple pass-through properties & methods.
		#region GPTrainingData_WrapAround

		public double this[int Row, int Column]
		{
			get { return m_Training[Row,Column]; }
			set { m_Training[Row,Column] = value; }
		}

		public bool TimeSeries
		{
			get { return m_Training.TimeSeries; }
			set { m_Training.TimeSeries = value; }
		}

		public int Rows
		{
			get { return m_Training.Rows; }
		}

		public int Columns
		{
			get { return m_Training.Columns; }
		}

		public int Objectives
		{
			get { return m_Training.Objectives; }
		}

		public double[] InputRow(int Row)
		{
			return m_Training.InputRow(Row);
		}

		public double[] ObjectiveRow(int Row)
		{
			return m_Training.ObjectiveRow(Row);
		}

		public double[] InputColumn(int Column)
		{
			return m_Training.InputColumn(Column);
		}

		public double[] ObjectiveColumn(int Column)
		{
			return m_Training.ObjectiveColumn(Column);
		}

		#endregion

		private int m_ModelingFileID;
		public int ModelingFileID
		{
			get { return m_ModelingFileID; }
		}

		private string m_sName;
		public string Name
		{
			get { return m_sName; }
			set { m_sName = value; }
		}

		private string m_sDescription;
		public string Description
		{
			get { return m_sDescription; }
			set { m_sDescription = value; }
		}

		private String[] m_Header;
		public String[] Header
		{
			get { return m_Header; }
		}

		/// <summary>
		/// Load a data set in ssv (Semi-Colon separated values) from a disk file and import it into the database.
		/// This is for european standard formats.
		/// </summary>
		/// <param name="Filename"></param>
		/// <param name="DelInit"></param>
		/// <param name="DelIncrement"></param>
		/// <returns></returns>
		public bool LoadSSV(String Filename, DELInitProgress DelInit, DELIncrementProgress DelIncrement)
		{
			try
			{
				m_Header = null;
				List<List<double>> TempData = new List<List<double>>();
				using (StreamReader reader = new StreamReader(new FileStream(Filename, FileMode.Open)))
				{
					//
					// Read the Training in
					int RowCount = 0;
					while (reader.EndOfStream == false)
					{
						String[] sFields = reader.ReadLine().Split(';');

						//
						// If this is the first row, perform an automatic detection
						// of header values.
						bool IsHeader = false;
						if (RowCount == 0)
						{
							IsHeader = ParseFileHeader(sFields);
						}
						if (!IsHeader)
						{
							//
							// Add a row
							TempData.Add(new List<double>());
							//
							// Add the values
							foreach (String Value in sFields)
							{
								double Number = Convert.ToDouble(Value);	// Use localized data format!
								TempData[RowCount].Add(Number);
							}
							RowCount++;
						}
					}
				}

				return ImportToDB(Filename, DelInit, DelIncrement, TempData);
			}
			catch
			{
#if GPLOG
				GPLog.ReportLine("ssv file read error",true);
#endif
				return false;
			}
		}

		public delegate void DELInitProgress(int MaxValue);
		public delegate void DELIncrementProgress();
		/// <summary>
		/// Load a data set in csv (commad separated values) from a disk file and import it into the database
		/// </summary>
		/// <param name="Filename">Name of file to load</param>
		/// <param name="DelInit">Delegate to call to indicate the initialization params</param>
		/// <param name="DelIncrement">Delete to call for each new row of data imported</param>
		/// <returns></returns>
		public bool LoadCSV(String Filename,DELInitProgress DelInit,DELIncrementProgress DelIncrement)
		{
			try
			{
				m_Header = null;
				List<List<double>> TempData = new List<List<double>>();
				using (StreamReader reader = new StreamReader(new FileStream(Filename, FileMode.Open)))
				{
					//
					// Read the Training in
					int RowCount = 0;
					while (reader.EndOfStream == false)
					{
						String[] sFields = reader.ReadLine().Split(',');

						//
						// If this is the first row, perform an automatic detection
						// of header values.
						bool IsHeader = false;
						if (RowCount == 0)
						{
							IsHeader = ParseFileHeader(sFields);
						}
						if (!IsHeader)
						{
							//
							// Add a row
							TempData.Add(new List<double>());
							//
							// Add the values
							foreach (String Value in sFields)
							{
								double Number = Convert.ToDouble(Value,GPUtilities.NumericFormat); // Enforce US "." format
								TempData[RowCount].Add(Number);
							}
							RowCount++;
						}
					}
				}

				return ImportToDB(Filename, DelInit, DelIncrement, TempData);
			}
			catch
			{
#if GPLOG
				GPLog.ReportLine("csv file read error",true);
#endif
				return false;
			}
		}

		/// <summary>
		/// Manages importing the data into the database
		/// </summary>
		/// <param name="Filename"></param>
		/// <param name="DelInit"></param>
		/// <param name="DelIncrement"></param>
		/// <param name="TempData"></param>
		/// <returns></returns>
		private bool ImportToDB(String Filename, DELInitProgress DelInit, DELIncrementProgress DelIncrement, List<List<double>> TempData)
		{
			//
			// Create the internal array and get the Training copied into it.  We
			// currently support a single objective, so a 1 for the last param.
			m_Training.ConstructStorage(TempData.Count, TempData[0].Count - 1, 1);
			for (int Row = 0; Row < TempData.Count; Row++)
			{
				//
				// Copy the values in
				for (int Column = 0; Column < (TempData[0].Count - 1); Column++)
				{
					m_Training[Row, Column] = TempData[Row][Column];
				}

				m_Training.ObjectiveRow(Row)[0] = TempData[Row][TempData[Row].Count - 1];
			}

			//
			// Decide if this is a time series file (single column)
			if (TempData[0].Count == 1)
			{
				this.TimeSeries = true;
			}
			else
			{
				this.TimeSeries = false;
			}

			//
			// Have the UI prepare itself 
			DelInit(this.Rows);

			//
			// Import this Training into the database - Let the user know the records
			// are being imported, that's what the 'DelIncrement' delegate is for.
			String[] FileParts = Filename.Split('\\');
			ImportDataToDB(FileParts[FileParts.Length - 1], Filename, DelIncrement);

			this.Name = FileParts[FileParts.Length - 1];
			this.Description = Filename;

			return true;
		}

		/// <summary>
		/// Performs an automatic detection to see if this row of Training is a 
		/// header row, if so, the values are recorded.  If no header row, then
		/// header values are created.
		/// </summary>
		/// <param name="sFields">header values to test</param>
		/// <returns>True/False upon success or failure</returns>
		private bool ParseFileHeader(string[] sFields)
		{
			//
			// Automatic detection technique is to see if all fields will convert
			// to doubles, if any one fails, then this is a header row.
			try
			{
				foreach (String Field in sFields)
				{
					double test = Convert.ToDouble(Field, GPUtilities.NumericFormat);
				}
			}
			catch (System.FormatException)
			{
				//
				// This is a header row - assign the strings to the header :)
				m_Header = sFields;
				return true;
			}

			//
			// If no header row, create headers
			if (m_Header == null)
			{
				m_Header = new String[sFields.Length];
				for (int item = 0; item < sFields.Length - 1; item++)
				{
					m_Header[item] = "Input " + (item + 1);
				}
				m_Header[sFields.Length - 1] = "Target";
			}

			return false;
		}

		/// <summary>
		/// Imports the current array of Training into the database
		/// </summary>
		/// <param name="Name">Name to record the Training set with</param>
		/// <param name="Description">Description to record the Training set with</param>
		/// <param name="DelIncrement">Delegate to call for indicating the status of the import</param>
		/// <returns>True/False upon success or failure</returns>
		private bool ImportDataToDB(String Name,String Description,DELIncrementProgress DelIncrement)
		{
			//
			// Get the DB connection
			using (OleDbConnection DBConnection = GPDatabaseUtils.Connect())
			{
				//
				// Add the Filename & Stats to tblModelingFile
				String sSQL = "INSERT INTO tblModelingFile(Name,Description,TimeSeries,ColumnCount,InputCount) ";
				sSQL = sSQL + "VALUES ('" + Name + "',";
				sSQL = sSQL + "'" + Description + "',";
				sSQL = sSQL + this.TimeSeries.ToString() + ",";
				sSQL = sSQL + "'" + (this.Columns+this.Objectives) + "',";
				if (this.TimeSeries)
				{
					sSQL = sSQL + "'1'";
				}
				else
				{
					sSQL = sSQL + "'" + this.Columns + "'";
				}
				sSQL = sSQL + ")";

				OleDbCommand cmd = new OleDbCommand("Add File", DBConnection);
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
					this.m_ModelingFileID = Convert.ToInt32(reader[0].ToString()); ;
					reader.Close();

					//
					// Construct a new command object to work with
					cmd.Dispose();
					cmd = new OleDbCommand("Add Data", DBConnection);

					//
					// Load the Training into the database
					for (int nRow = 0; nRow < this.Rows; nRow++)
					{
						//
						// Write the inputs
						for (int nCell = 0; nCell < this.Columns; nCell++)
						{
							sSQL = "INSERT INTO tblModelingFileColumn(ModelingFileID,RowIndex,ColumnIndex,ValueDouble) ";
							sSQL = sSQL + "VALUES (" + ModelingFileID + ",";
							sSQL = sSQL + nRow + ",";
							sSQL = sSQL + nCell + ",";
							sSQL = sSQL + Convert.ToString(this[nRow, nCell],GPUtilities.NumericFormat) + "";
							sSQL = sSQL + ")";

							cmd.CommandText = sSQL;
							cmd.CommandType = CommandType.Text;
							cmd.ExecuteNonQuery();
						}

						//
						// Write the objectives 
						for (int Objective = 0; Objective < this.Objectives; Objective++)
						{
							sSQL = "INSERT INTO tblModelingFileColumn(ModelingFileID,RowIndex,ColumnIndex,ValueDouble) ";
							sSQL = sSQL + "VALUES (" + ModelingFileID + ",";
							sSQL = sSQL + nRow + ",";
							sSQL = sSQL + (this.Columns+Objective) + ",";
							sSQL = sSQL + Convert.ToString(this.ObjectiveRow(nRow)[Objective],GPUtilities.NumericFormat) +"";
							sSQL = sSQL + ")";

							cmd.CommandText = sSQL;
							cmd.CommandType = CommandType.Text;
							cmd.ExecuteNonQuery();
						}

						//
						// Let the UI know it is okay to update something
						DelIncrement();
					}

					//
					// Save the column headers (if they exit)
					if (m_Header != null)
					{
						for (int item = 0; item < m_Header.Length; item++)
						{
							sSQL = "INSERT INTO tblModelingFileHeader(ModelingFileID,ColumnIndex,Name) ";
							sSQL=sSQL+"VALUES("+ModelingFileID+",";
							sSQL=sSQL+item+",";
							sSQL=sSQL+"'"+m_Header[item]+"'";
							sSQL=sSQL+")";

							cmd.CommandText = sSQL;
							cmd.CommandType = CommandType.Text;
							cmd.ExecuteNonQuery();
						}
					}
				}
				catch (OleDbException ex)
				{
					string sError = ex.Message.ToString();
				}
			}

			return true;
		}

		/// <summary>
		/// Load a data set from the database.  This method also provides delegates
		/// that can be called to allow the UI to update as the data is loaded.
		/// </summary>
		/// <param name="ModelingFileID"></param>
		/// <param name="DelInit"></param>
		/// <param name="DelIncrement"></param>
		/// <returns></returns>
		public bool LoadFromDB(int ModelingFileID, DELInitProgress DelInit, DELIncrementProgress DelIncrement)
		{
			m_ModelingFileID = ModelingFileID;

			//
			// Get the DB connection
			using (OleDbConnection DBConnection = GPDatabaseUtils.Connect())
			{

				//
				// Read the header information
				int FileRows = 0;
				int FileInputs = 0;
				int FileObjectives = 0;
				LoadHeaderDB(DBConnection, ModelingFileID, ref FileInputs, ref FileObjectives);

				//
				// Find out how many records we will get back first, this is needed to construct the data storage
				String SQLRowCount="SELECT COUNT(ValueDouble) FROM tblModelingFileColumn WHERE ModelingFileID = " + ModelingFileID;
				OleDbCommand cmd = new OleDbCommand(SQLRowCount, DBConnection);
				int RowCount = (int)cmd.ExecuteScalar();


				//
				// Determine the number of rows of data - The data is stored as a single
				// column and we have to transform it into rows right here, that is why
				// the rows have to be computed based upon the number of columns.
				FileRows = RowCount / (FileInputs + FileObjectives);

				//
				// Let the calling object know the size
				if (DelInit != null)
					DelInit(FileRows);

				//
				// Create the storage arrays
				m_Training.ConstructStorage(FileRows, FileInputs, FileObjectives);

				//
				// Create the query
				String SQL = "SELECT ValueDouble FROM tblModelingFileColumn WHERE ModelingFileID = " + ModelingFileID + " ORDER BY RowIndex,ColumnIndex";
				cmd = new OleDbCommand(SQL, DBConnection);
				using (OleDbDataReader rdr = cmd.ExecuteReader())
				{
					rdr.Read();

					//
					// Grab the values
					for (int Row = 0; Row < FileRows; Row++)
					{
						for (int Column = 0; Column < FileInputs; Column++)
						{
							m_Training[Row, Column] = (double)rdr["ValueDouble"];
							rdr.Read();
						}
						for (int Objective = 0; Objective < FileObjectives; Objective++)
						{
							m_Training.ObjectiveRow(Row)[Objective] = (double)rdr["ValueDouble"];
							rdr.Read();
						}

						//
						// Update the calling object
						if (DelIncrement != null)
							DelIncrement();
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Load the file header information, which describes what data to 
		/// expect for loading
		/// </summary>
		/// <param name="DBConnection"></param>
		/// <param name="DBCode"></param>
		/// <param name="FileInputs"></param>
		/// <param name="FileObjectives"></param>
		/// <returns>True/False upon success or failure to read the header</returns>
		private bool LoadHeaderDB(OleDbConnection DBConnection, int DBCode,ref int FileInputs,ref int FileObjectives)
		{
			String SQL = "SELECT Name,Description,TimeSeries,ColumnCount,InputCount FROM tblModelingFile WHERE DBCode = " + DBCode;
			OleDbCommand cmd = new OleDbCommand(SQL, DBConnection);
			OleDbDataReader rdr = cmd.ExecuteReader();
			//
			// Ensure we got something from the DB
			if (!rdr.HasRows) return false;
			rdr.Read();


			this.Name = rdr["Name"].ToString();
			this.Description = rdr["Description"].ToString();
			this.TimeSeries = (bool)rdr["TimeSeries"]; 
			int TotalColumns = (short)rdr["ColumnCount"]; 
			FileInputs = (short)rdr["InputCount"]; 
			FileObjectives = TotalColumns - FileInputs;

			//
			// Read column headers, if they exist
			// TODO: Eventually can remove test for no header once all files get headers added to them
			String sSQLHeaders = "SELECT Name FROM tblModelingFileHeader WHERE ModelingFileID = " + DBCode+" ORDER BY ColumnIndex";
			OleDbDataAdapter daHeader = new OleDbDataAdapter(sSQLHeaders, DBConnection);
			DataSet dSet = new DataSet();
			daHeader.Fill(dSet);

			if (dSet.Tables[0].Rows.Count > 0)
			{
				m_Header = new String[dSet.Tables[0].Rows.Count];
				for (int item = 0; item < m_Header.Length; item++)
				{
					m_Header[item] = dSet.Tables[0].Rows[item].ItemArray.GetValue(0).ToString().Trim();
				}
			}
			else
			{
				m_Header = new String[TotalColumns];
				for (int Input = 0; Input < FileInputs; Input++)
				{
					m_Header[Input] = "Input " + (Input + 1);
				}
				for (int Target = 0; Target < FileObjectives; Target++)
				{
					m_Header[FileInputs + Target] = "Target " + (Target + 1);
				}
			}

			return true;
		}

		/// <summary>
		/// Remove the indicated Training set from the database
		/// </summary>
		/// <param name="ModelingFileID">DBCode of the Training set to delete</param>
		/// <returns>True/False upon success or failure</returns>
		public static bool DeleteData(int ModelingFileID)
		{
			//
			// Get the DB connection
			using (OleDbConnection DBConnection = GPDatabaseUtils.Connect())
			{

				String sSQL = "DELETE FROM tblModelingFile WHERE DBCode = " + ModelingFileID;

				OleDbCommand cmd = new OleDbCommand("Delete File", DBConnection);
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
			}

			return true;
		}

		/// <summary>
		/// Checks to see if this file is in use by a project
		/// </summary>
		/// <param name="ModelingFileID">DBCode of the file to check out</param>
		/// <returns>True/False depending upon if the file is being used</returns>
		public static bool FileInUse(int ModelingFileID)
		{
			using (OleDbConnection con = GPDatabaseUtils.Connect())
			{

				//
				// Start by checking the project table
				string sSQL = "SELECT DBCode FROM tblProject ";
				sSQL += "WHERE TrainingFileID = " + ModelingFileID;
				OleDbCommand cmd = new OleDbCommand(sSQL, con);
				OleDbDataReader rdr = cmd.ExecuteReader();
				if (rdr.HasRows)
				{
					return true;
				}

				//
				// Next, check the training file table
				sSQL = "SELECT DBCode FROM tblValidationFiles WHERE ModelingFileID = " + ModelingFileID;
				cmd = new OleDbCommand(sSQL, con);
				rdr = cmd.ExecuteReader();
				if (rdr.HasRows)
				{
					return true;
				}
			}

			//
			// Not found, so return false
			return false;
		}
	}
}
