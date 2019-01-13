using System;
using System.IO;
using System.Reflection;
using System.Text;

//from https://www.cnblogs.com/yangsirc/p/8295129.html

namespace Gongfu_World_Console.Scripts
{

    public enum LogType
    {
        Undefined,
        All,
        Info,
        Debug,
        Success,
        Failure,
        Warning,
        Error,
        Csv
    }


    public class Logger
    {
        public class LoggerInstance
        {
            private readonly object _logLock;

            private readonly string _logFileName;

            private LogType _logType = LogType.Info;

            public LogType LogType { get => _logType; set => _logType = value; }

            public LoggerInstance(string fileName, bool isOverWrite = false)
            {
                _logFileName = fileName;
                _logLock = new object();
                if (isOverWrite && File.Exists(_logFileName))
                {
                    FileStream fs = new FileStream(_logFileName, FileMode.Truncate, FileAccess.Write);
                    fs.Close();               
                }
            }


            /// <summary>
            /// Write log to log file
            /// </summary>
            /// <param name="logContent">Log content</param>
            /// <param name="logType">Log type</param>
            public void WriteLog(string logContent, LogType logType = LogType.Undefined)
            {
                try
                {
                    string[] logText;

                    if (logType == LogType.Undefined)
                    {
                        logType = LogType;
                    }

                    if (logType != LogType.Csv)
                    {
                        logText = new string[] { DateTime.Now.ToString("hh:mm:ss") + ": " + logType + ": " + logContent };
                    }
                    else
                    {
                        logText = new string[] { logContent };
                    }


                    File.AppendAllLines(_logFileName, logText, Encoding.Default);

                }
                catch (Exception)
                {
                    // ignored
                }
            }

            #region WriteLog Origin Edition
            /*
            /// <summary>
            /// Write log to log file
            /// </summary>
            /// <param name="logContent">Log content</param>
            /// <param name="logType">Log type</param>
            public void WriteLog(string logContent, LogType logType = LogType.Information, string fileName = null)
            {
                try
                {
                    string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    basePath = @"C:\APILogs";
                    if (!Directory.Exists(basePath + "\\Log"))
                    {
                        Directory.CreateDirectory(basePath + "\\Log");
                    }

                    string dataString = DateTime.Now.ToString("yyyy-MM-dd");
                    if (!Directory.Exists(basePath + "\\Log\\" + dataString))
                    {
                        Directory.CreateDirectory(basePath + "\\Log\\" + dataString);
                    }

                    string[] logText = new string[] { DateTime.Now.ToString("hh:mm:ss") + ": " + logType.ToString() + ": " + logContent };
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        fileName = fileName + "_" + _logFileName;
                    }
                    else
                    {
                        fileName = _logFileName;
                    }

                    lock (_logLock)
                    {
                        File.AppendAllLines(basePath + "\\Log\\" + dataString + "\\" + fileName, logText);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }*/
            #endregion

            /// <summary>
            /// Write exception to log file
            /// </summary>
            /// <param name="exception">Exception</param>
            public void WriteException(Exception exception, string specialText = null)
            {
                if (exception != null)
                {
                    Type exceptionType = exception.GetType();
                    string text = string.Empty;
                    if (!string.IsNullOrEmpty(specialText))
                    {
                        text = text + specialText + Environment.NewLine;
                    }
                    text = "Exception: " + exceptionType.Name + Environment.NewLine;
                    text += "               " + "Message: " + exception.Message + Environment.NewLine;
                    text += "               " + "Source: " + exception.Source + Environment.NewLine;
                    text += "               " + "StackTrace: " + exception.StackTrace + Environment.NewLine;
                    WriteLog(text, LogType.Error);
                }
            }
        }

        private static LoggerInstance _error;

        private static LoggerInstance _debug;

        private static LoggerInstance _csv;

        private static LoggerInstance _dmg_csv;

        private Logger() { }

        /// <summary>
        /// Logger Error instance
        /// </summary>
        public static LoggerInstance Error
        {
            get
            {
                if (_error == null)
                {
                    //logFileName = Guid.NewGuid() + ".log";
                    _error = new LoggerInstance(Find.LogPath + "Error.log");
                }
                return _error;
            }
        }

        /// <summary>
        /// Logger Debug instance
        /// </summary>
        public static LoggerInstance Debug
        {
            get
            {
                if (_debug == null)
                {
                    _debug = new LoggerInstance(Find.LogPath + "Debug.log");
                }
                return _debug;
            }
        }

        /// <summary>
        /// Logger Csv instance
        /// </summary>
        public static LoggerInstance Csv
        {
            get
            {
                if (_csv == null)
                {
                    //logFileName = Guid.NewGuid() + ".log";
                    _csv = new LoggerInstance(Find.LogPath + "Debug.csv", isOverWrite: true);
                    _csv.LogType = LogType.Csv;
                }
                return _csv;
            }
        }

        public static LoggerInstance DmgCsv
        {
            get
            {
                if (_dmg_csv == null)
                {
                    //logFileName = Guid.NewGuid() + ".log";
                    _dmg_csv = new LoggerInstance(Find.LogPath + "Damage_Debug.csv", isOverWrite: true);
                    _dmg_csv.LogType = LogType.Csv;
                }
                return _dmg_csv;
            }

        }
    }
}