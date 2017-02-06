﻿// DataSelector is an ArcGIS add-in used to extract biodiversity
// information from SQL Server based on any selection criteria.
//
// Copyright © 2016 Sussex Biodiversity Record Centre
//
// This file is part of DataSelector.
//
// DataSelector is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// DataSelector is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with DataSelector.  If not, see <http://www.gnu.org/licenses/>.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HLFileFunctions
{
    class FileFunctions
    {
        public bool DirExists(string aFilePath)
        {
            // Check input first.
            if (aFilePath == null) return false;
            DirectoryInfo myDir = new DirectoryInfo(aFilePath);
            if (myDir.Exists == false) return false;
            return true;
        }

        public string GetDirectoryName(string aFullPath)
        {
            // Check input.
            if (aFullPath == null) return null;

            // split at the last \
            int LastIndex = aFullPath.LastIndexOf(@"\");
            string aPath = aFullPath.Substring(0, LastIndex);
            return aPath;
        }

        #region FileExists
        public bool FileExists(string aFilePath, string aFileName)
        {
            if (DirExists(aFilePath))
            {
                string strFileName = aFilePath;
                string aTest = aFilePath.Substring(aFilePath.Length - 1, 1);
                if (aTest != @"\")
                {
                    strFileName = strFileName + @"\" + aFileName;
                }
                else
                {
                    strFileName = strFileName + aFileName;
                }

                System.IO.FileInfo myFileInfo = new FileInfo(strFileName);

                if (myFileInfo.Exists) return true;
                else return false;
            }
            return false;
        }
        public bool FileExists(string aFullPath)
        {
            System.IO.FileInfo myFileInfo = new FileInfo(aFullPath);
            if (myFileInfo.Exists) return true;
            return false;
        }

        #endregion

        public string GetFileName(string aFullPath)
        {
            // Check input.
            if (aFullPath == null) return null;

            // split at the last \
            int LastIndex = aFullPath.LastIndexOf(@"\");
            string aFile = aFullPath.Substring(LastIndex + 1, aFullPath.Length - (LastIndex + 1));
            return aFile;
        }

        public string ReturnWithoutExtension(string aFileName)
        {
            // check input
            if (aFileName == null) return null;
            int aLen = aFileName.Length;
            // check if it has an extension at all
            string aTest = aFileName.Substring(aLen - 4, 1);
            if (aTest != ".") return aFileName;

            return aFileName.Substring(0, aLen - 4);
        }

        public bool DeleteFile(string aFullPath)
        {
            if (FileExists(aFullPath))
            {
                try
                {
                    File.Delete(aFullPath);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
                return true;

        }

        public bool CreateLogFile(string aTextFile)
        {
            StreamWriter myWriter = new StreamWriter(aTextFile, false);

            myWriter.WriteLine("Log file for Stored Procedure Runner, started on " + DateTime.Now.ToString());
            myWriter.Close();
            myWriter.Dispose();
            return true;
        }

        public bool WriteLine(string aTextFile, string aWriteLine)
        {
            StreamWriter myWriter = new StreamWriter(aTextFile, true);
            aWriteLine = DateTime.Now.ToString() + " : " + aWriteLine;
            myWriter.WriteLine(aWriteLine);
            myWriter.Close();
            myWriter.Dispose();
            return true;
        }
    }
}
