using System;
using System.IO;
using System.Reflection;

//from https://www.cnblogs.com/yangsirc/p/8295129.html

namespace Gongfu_World_Console.Scripts
{

    public enum LogType
    {
        All,
        Information,
        Debug,
        Success,
        Failure,
        Warning,
        Error
    }


    public class Logger
    {
        public class LoggerInstance
        {
            private readonly object _logLock;

            private readonly string _logFileName;

            public LoggerInstance(string fileName)
            {
                _logFileName = fileName;
                _logLock = new object();
            }


            /// <summary>
            /// Write log to log file
            /// </summary>
            /// <param name="logContent">Log content</param>
            /// <param name="logType">Log type</param>
            public void WriteLog(string logContent, LogType logType = LogType.Information)
            {
                try
                {
                    string[] logText = new string[] { DateTime.Now.ToString("hh:mm:ss") + ": " + logType.ToString() + ": " + logContent };

                    File.AppendAllLines(_logFileName, logText);

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


    }
}