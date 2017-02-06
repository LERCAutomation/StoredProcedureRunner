using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HLSQLServerFunctions;
using HLFileFunctions;
using System.Data.SqlClient;

namespace StoredProcedureRunner
{
    public partial class frmRunSP : Form
    {
        SPRunnerConfig myConfig;
        SQLServerFunctions mySQLServerFuncs;
        FileFunctions myFileFuncs;
        bool blOpenForm;
        public frmRunSP()
        {
            InitializeComponent();
            blOpenForm = true;
            // Fill with the relevant.
            myConfig = new SPRunnerConfig(); // Should find the config file automatically.

            // Check if the config file has loaded correctly
            if (myConfig.FoundXML() == false)
            {
                MessageBox.Show("XML file not found; form cannot load.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                blOpenForm = false;
            }
            else if (myConfig.LoadedXML() == false)
            {
                MessageBox.Show("Error loading XML File; form cannot load.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                blOpenForm = false;
            }

            // We have all the checks in place. See if we can open the SQL connection.
            mySQLServerFuncs = new SQLServerFunctions();
            SqlConnection conConnection = null;

            if (blOpenForm)
            {
                try
                {
                    conConnection = mySQLServerFuncs.CreateSQLConnection(myConfig.ConnectionString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot open SQL connection " + myConfig.ConnectionString() + ". Error is: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    blOpenForm = false;
                }

                // Open and close the connection to make sure it works.
                try
                {
                    conConnection.Open();
                    conConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot open the database " + conConnection.Database + ". Error is: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    blOpenForm = false;
                }
            }

            // Now open the form, or not.
            if (blOpenForm)
            {
                lblDBase.Text = "Database name: " + conConnection.Database;
                lblSPName.Text = "Stored procedure: " + myConfig.SPName();
            }
            else
            {
                Load += (s, e) => Close();
                return;
            }
            conConnection = null;
            myFileFuncs = new FileFunctions();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Code to run the stored procedure.
            
            // Start log file.
            string strLogFileName = "StoredProcedureRun" + DateTime.Now.Year.ToString() + "_" +  DateTime.Now.Month.ToString("0#") + "_" +  DateTime.Now.Day.ToString("0#") + ".txt";
            strLogFileName = myConfig.LogFilePath() + "\\" + strLogFileName;
            myFileFuncs.CreateLogFile(strLogFileName);
            myFileFuncs.WriteLine(strLogFileName, "Database connection string: " + myConfig.ConnectionString());
            myFileFuncs.WriteLine(strLogFileName, "Stored procedure: " + myConfig.SPName());
            if (myConfig.SPVariables().Count > 0)
            {
                string strTemp = "Variables entered: ";
                int aCount = 0;
                foreach (string strVar in myConfig.SPVariables())
                {
                    strTemp = strTemp + myConfig.SPVariables()[0];
                    aCount++;
                    if (aCount < myConfig.SPVariables().Count)
                    {
                        strTemp = strTemp + ";";
                    }
                    else
                    {
                        strTemp = strTemp + ".";
                    }
                }
                myFileFuncs.WriteLine(strLogFileName, strTemp);
            }
            if (myConfig.OutputTable() != "")
            {
                myFileFuncs.WriteLine(strLogFileName, "Output table: " + myConfig.OutputTable());
            }

            // Open the connection
            SqlConnection conConnection = null;
            conConnection = mySQLServerFuncs.CreateSQLConnection(myConfig.ConnectionString());
            conConnection.Open();

            myFileFuncs.WriteLine(strLogFileName, "Connection to database " + conConnection.Database + " opened successfully");

            // Define the stored procedure
            SqlCommand cmdStoredProcedure = new SqlCommand(myConfig.DBSchema() + "." + myConfig.SPName(), conConnection);
            cmdStoredProcedure.CommandType = CommandType.StoredProcedure;

            myFileFuncs.WriteLine(strLogFileName, "Stored procedure " + myConfig.SPName() + " defined successfully");

            // Add any variables to the SP as required.
            List<string> liVarStrings = myConfig.SPVariables();

            foreach (string strVar in liVarStrings)
            {
                //decide what the variable type is:
                List<string> liThisVar = strVar.Split(',').ToList();
                bool blBool = false;
                bool blInt = false;
                int intValue;
                bool blValue;

                blInt = int.TryParse(liThisVar[1],out intValue);
                blBool = bool.TryParse(liThisVar[1], out blValue);
                if (blInt)
                {
                    mySQLServerFuncs.AddSQLParameter(ref cmdStoredProcedure, liThisVar[0], intValue);
                }
                else if (blBool)
                {
                    mySQLServerFuncs.AddSQLParameter(ref cmdStoredProcedure, liThisVar[0], blValue);
                }
                else // Treat as a string for now.
                {
                    mySQLServerFuncs.AddSQLParameter(ref cmdStoredProcedure, liThisVar[0], liThisVar[1]);
                }

            }

            myFileFuncs.WriteLine(strLogFileName, "Variables added to the stored procedure successfully");

            // Run the stored procedure.
            bool blQueryHasRun = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                cmdStoredProcedure.ExecuteNonQuery();
                myFileFuncs.WriteLine(strLogFileName, "Stored procedure executed successfully");
                blQueryHasRun = true;
                if (myConfig.OutputTable() != "")
                {
                    int intRecordCount = mySQLServerFuncs.CountRows(ref conConnection, myConfig.OutputTable());
                    if (intRecordCount == 1)
                    {
                        MessageBox.Show("The result table " + myConfig.OutputTable() + " has one record");
                        myFileFuncs.WriteLine(strLogFileName, "The result table " + myConfig.OutputTable() + " has one record");
                    }
                    else
                    {
                        MessageBox.Show("The result table " + myConfig.OutputTable() + " has " + intRecordCount.ToString() + " records");
                        myFileFuncs.WriteLine(strLogFileName, "The result table " + myConfig.OutputTable() + " has " + intRecordCount.ToString() + " records");
                    }
                }
                else
                {
                    MessageBox.Show("The stored procedure was executed successfully");
                }
                

            }
            catch (Exception ex)
            {
                if (blQueryHasRun)
                {
                    MessageBox.Show("The stored procedure was executed successfully. However, the tool cannot display results because " +
                        "the result table name " + myConfig.OutputTable() + " in the XML file is not valid. System error message: " + ex.Message);
                    myFileFuncs.WriteLine(strLogFileName, "ERROR: The output table name " + myConfig.OutputTable() + " is not correct. System error message: " + ex.Message);
                        
                }
                else
                {
                    MessageBox.Show("Cannot execute stored procedure " + myConfig.SPName() + ". Error is: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    myFileFuncs.WriteLine(strLogFileName, "ERROR: Cannot execute stored procedure " + myConfig.SPName() + ". Error is: " + ex.Message);
                }
            }
            finally
            {
                conConnection.Close();
                myFileFuncs.WriteLine(strLogFileName, "Connection to database closed");
                cmdStoredProcedure = null;
                conConnection = null;
                this.Cursor = Cursors.Default;
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
