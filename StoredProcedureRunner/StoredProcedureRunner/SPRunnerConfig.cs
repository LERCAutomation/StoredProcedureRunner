using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using System.Windows.Forms;
using HLFileFunctions;
using StoredProcedureRunner.Properties;

namespace StoredProcedureRunner
{
   
    class SPRunnerConfig
    {
        // Declare all variables.
        string strConnectionString;
        string strLogFilePath;
        string strSPName;
        string strSPVariables;
        List<string> liInputVariables = new List<string>();
        string strOutputTable = "";
        string strSchema;
        int intTimeOut;

        bool blFoundXML;
        bool blLoadedXML;


        // Load the XML if possible.
        FileFunctions myFileFuncs;
        XmlElement xmlSPRunner;
        public SPRunnerConfig()
        {
            // Open xml
            myFileFuncs = new FileFunctions();
            string strXMLFile = null;
            blLoadedXML = true;
            try
            {
                // Get the XML file
                strXMLFile = Settings.Default.XMLFile;

                // If the XML file path is blank or doesn't exist
                if (String.IsNullOrEmpty(strXMLFile) || (!myFileFuncs.FileExists(strXMLFile)))
                {
                    // Prompt the user for the correct file path
                    string strFolder = GetConfigFilePath();
                    if (!String.IsNullOrEmpty(strFolder))
                        strXMLFile = strFolder + @"\SPRunner.xml";
                }

                // Check the xml file path exists
                if (myFileFuncs.FileExists(strXMLFile))
                {
                    Settings.Default.XMLFile = strXMLFile;
                    Settings.Default.Save();
                    blFoundXML = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error " + ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Read the file.
            if (blFoundXML)
            {
                XmlDocument xmlConfig = new XmlDocument();
                xmlConfig.Load(strXMLFile);

                XmlNode currNode = xmlConfig.DocumentElement.FirstChild; // This gets us the DataSelector.
                xmlSPRunner = (XmlElement)currNode;


                // Get all of the detail into the object
                try
                {
                    strLogFilePath = xmlSPRunner["LogFilePath"].InnerText;
                }
                catch
                {
                    MessageBox.Show("Could not locate the item 'LogFilePath' in the XML file");
                    blLoadedXML = false;
                    return;
                }

                try
                {
                    strConnectionString = xmlSPRunner["ConnectionString"].InnerText;
                }
                catch
                {
                    MessageBox.Show("Could not locate the item 'ConnectionString' in the XML file");
                    blLoadedXML = false;
                    return;
                }

                try
                {
                    strSchema = xmlSPRunner["DBSchema"].InnerText;
                }
                catch
                {
                    MessageBox.Show("Could not locate the item 'DBSchema' in the XML file");
                    blLoadedXML = false;
                    return;
                }

                try
                {
                    string strTimeout = xmlSPRunner["TimeoutSeconds"].InnerText;
                    bool blSuccess;

                    if (strTimeout != "")
                    {

                        blSuccess = int.TryParse(strTimeout, out intTimeOut);
                        if (!blSuccess)
                        {
                            MessageBox.Show("The value entered for TimeoutSeconds in the XML file is not an integer number");
                            blLoadedXML = false;
                        }
                    }
                    else
                    {
                        intTimeOut = 0; // None given.
                    }
                    
                }
                catch
                {
                    MessageBox.Show("Could not locate the item 'DBSchema' in the XML file");
                    blLoadedXML = false;
                    return;
                }

                try
                {
                    strSPName = xmlSPRunner["SPName"].InnerText;
                }
                catch
                {
                    MessageBox.Show("Could not locate the item 'SPName' in the XML file");
                    blLoadedXML = false;
                    return;
                }

                try
                {
                    strSPVariables = xmlSPRunner["SPVariables"].InnerText;
                    if (strSPVariables != "")
                    {
                        // Process into a list
                        liInputVariables = strSPVariables.Split(';').ToList();
                        // Test if there are two parts to each variable; bail out if not.
                        foreach (string strVar in liInputVariables)
                        {
                            List<string> liVar = strVar.Split(',').ToList();
                            int i = 0;
                            foreach (string strElement in liVar)
                            {
                                i++;
                            }
                            if (i != 2)
                            {
                                MessageBox.Show("There is an incorrect number of elements for variable " + liVar[0] + ". There are " + i.ToString() + " elements instead of two.");
                                blLoadedXML = false;
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Could not locate the item 'SPVariables' in the XML file");
                    blLoadedXML = false;
                    return;
                }

                try
                {
                    strOutputTable = xmlSPRunner["ResultTable"].InnerText;
                }
                catch
                {
                    MessageBox.Show("Could not locate the item 'ResultTable' in the XML file");
                    blLoadedXML = false;
                    return;
                }

            }
        }

        // Return all of the variables in the XML
        public bool FoundXML()
        {
            return blFoundXML;
        }

        public bool LoadedXML()
        {
            return blLoadedXML;
        }

        public string LogFilePath()
        {
            return strLogFilePath;
        }

        public string ConnectionString()
        {
            return strConnectionString;
        }

        public string DBSchema()
        {
            return strSchema;
        }

        public int TimeoutSeconds()
        {
            return intTimeOut;
        }

        public string SPName()
        {
            return strSPName;
        }

        public List<string> SPVariables()
        {
            return liInputVariables;
        }

        public string OutputTable()
        {
            return strOutputTable;
        }

        private string GetConfigFilePath()
        {
            // Create folder dialog.
            FolderBrowserDialog xmlFolder = new FolderBrowserDialog();

            // Set the folder dialog title.
            xmlFolder.Description = "Select folder containing 'SPRunner.xml' file ...";
            xmlFolder.ShowNewFolderButton = false;

            // Show folder dialog.
            if (xmlFolder.ShowDialog() == DialogResult.OK)
            {
                // Return the selected path.
                return xmlFolder.SelectedPath;
            }
            else
                return null;
        }
    }
}
