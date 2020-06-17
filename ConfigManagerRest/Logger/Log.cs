using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ConfigManagerRest.Logger
{
    #region Logger
    internal class Log
    {
        #region user declare
        private static int fileIndex;
        internal static object locker = new object();

        public enum LogType : int
        {
            Information = 0, Exception = 1
        }

        public enum LogLevel : int
        {
            Level0 = 0, Level1 = 1, Level2 = 2, Level3 = 3, Level4 = 4, Level5 = 5, Level6 = 6, Level7 = 7, Level8 = 8, Level9 = 9, Level10 = 10, Level50 = 50, Level100 = 100, Level1000 = 1000

        }

        public static int TraceLevel = 0;
        public static string TraceProcess = "";
        public static string TracePath = "";
        public static string TraceFileName = "";
        #endregion

        #region LogFileName
        private static string LogFileName(string dirPath, string fileName)
        {
            string newFileName = "";
            var fn = fileName.Split('.');
            fileName = fn[0];

            try
            {
                if (fileIndex == 0)
                    fileIndex = 1;

                if (!Directory.Exists(dirPath))
                {
                    fileIndex = 1;
                    Directory.CreateDirectory(dirPath);
                }

                newFileName = Path.Combine(dirPath, fileName + "_" + fileIndex.ToString().PadLeft(4, '0') + ".log");

                if (!File.Exists(newFileName))
                    return newFileName;

                var l_currentFileInfo = new FileInfo(newFileName);
                if (l_currentFileInfo.Length > 100 * 1024 * 1024)
                {
                    ++fileIndex;
                    newFileName = LogFileName(dirPath, fileName);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
            }
            return newFileName;
        }
        #endregion

        #region WriteLog
        public static void WriteLog(LogLevel logLevel, LogType logType, string ThreadId, string ProcessName, string loginformation)
        {
            if (logType != LogType.Exception)
            {
                if ((int)logLevel > (int)TraceLevel && !TraceProcess.Contains("-" + ProcessName + "-"))
                    return;
            }

            lock (locker)
            {
                StreamWriter oStreamWriter = null;
                try
                {
                    string strFilePath = "";

                    if (TracePath != "")
                        strFilePath = TracePath + "\\" + string.Format("{0:yyyyMMdd}", DateTime.Now) + "\\";
                    else
                        strFilePath = AppDomain.CurrentDomain.BaseDirectory + @"..\Log\" +
                                             string.Format("{0:yyyyMMdd}", DateTime.Now) + "\\";

                    string filepath = LogFileName(strFilePath, TraceFileName);
                    oStreamWriter = new StreamWriter(filepath, true);
                    oStreamWriter.WriteLine(System.DateTime.Now.ToString() + " - " + "ThreadId : " + ThreadId + " - " + ProcessName + " - " + loginformation);

                }
                catch (Exception)
                {
                    //Exception
                }
                finally
                {
                    if (oStreamWriter != null)
                    {
                        oStreamWriter.Close();
                    }
                }
            }
        }
        #endregion
    }
    #endregion
}