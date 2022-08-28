////using GlucoMan;
using System;
using System.Diagnostics;
using System.IO;

namespace gamon
{
    public class Logger
    {
        private string commonPath = "./";
        private string eventsLogFile = "logger.txt";
        private string dataLogFile = "logger.txt";
        private string errorsFile = "logger.txt";
        private string debugFile = "logger.txt";
        private string promptsFile = "logger.txt";
         
        /// <summary>
        /// No parameters constructor, uses predefined fields 
        /// </summary>
        public Logger()
        {
            eventsLogFile = Path.Combine(commonPath , eventsLogFile);
            dataLogFile = Path.Combine(commonPath , dataLogFile);
            errorsFile = Path.Combine(commonPath , errorsFile);
            debugFile = Path.Combine(commonPath , debugFile);

            defaultProperties();
        }
        #region Properties
        public string EventsLogFile { get => eventsLogFile;}
        public string DataLogFile { get => dataLogFile;}
        public string ErrorsFile { get => errorsFile;}
        public string DebugFile { get => debugFile;}
        public string PromptsFile { get => promptsFile;}
        public bool ShowingEvents { get; set; }
        public bool ShowingData { get; set; }
        public bool ShowingErrors { get; set; }
        public bool ShowingDebug { get; set; }
        public bool ShowingPrompts { get; set; }
        public bool LoggingEvents { get; set; }
        public bool LoggingData { get; set; }
        public bool LoggingErrors { get; set; }
        public bool LoggingDebug { get; set; }
        public bool LoggingPrompts { get; set; }
        #endregion
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="CommonPath">Path added to all filenames</param>
        /// <param name="LogFile">[Path] & filename for common events (if "" => no log)</param>
        /// <param name="ErrorFile">[Path] & filename for errors (if "" => no log)</param>
        /// <param name="TestFile">[Path] & filename for debugging (if "" => no log)</param>
        /// <param name="PromptsFile">[Path] & filename for console prompts (if "" => no log)</param>
        public Logger (string CommonPath, bool ShowAll, string EventLogFile, string ErrorFile, 
            string DebugFile, string PromptsFile, string DataLogFile)
        {
            defaultProperties();

            LoggingEvents = (EventLogFile != "" && EventLogFile != null);
            LoggingErrors = (ErrorFile != "" && ErrorFile != null);
            LoggingDebug = (DebugFile != "" && DebugFile != null);
            LoggingPrompts = (PromptsFile != "" && PromptsFile != null);
            LoggingEvents = (EventLogFile != "" && EventLogFile != null);
            LoggingData = (DataLogFile != "" && DataLogFile != null) ;
            
            commonPath = CommonPath;
            eventsLogFile = Path.Combine(commonPath , EventLogFile);
            errorsFile = Path.Combine(commonPath , ErrorFile);
            debugFile = Path.Combine(commonPath , DebugFile);
            promptsFile = Path.Combine(commonPath , PromptsFile);
            dataLogFile = Path.Combine(commonPath , DataLogFile);

            if (ShowAll)
            {
                ShowingEvents = true;
                ShowingErrors = true;
                ShowingDebug = true;
            }
        }
        /// <summary>
        /// Sets default properties, to be called by constructors
        /// </summary>
        private void defaultProperties()
        {
            ShowingEvents = false; 
            ShowingErrors = true;
            ShowingDebug = false;
        }
        /// <summary>
        /// Logs significative events
        /// </summary>
        /// <param name="testo"></param>
        public void Event(string testo)
        {
            if (LoggingEvents)
            {
                LogToFile(EventsLogFile, testo);
            }
            if (ShowingEvents)
            { 
                Console.Out.WriteLine(testo); 
            }
        }
        public void Data(string testo)
        {
            if (LoggingData)
            {
                using (StreamWriter sw = File.AppendText(dataLogFile))
                {
                    sw.WriteLine(testo);
                    sw.Close();
                }
            }
            if (ShowingData)
            {
                Console.Out.WriteLine(testo);
            }
        }
        /// <summary>
        /// Realizza il log degli errori
        /// </summary>
        /// <param name="ErrorText"></param>
        public string Error(string ErrorText, Exception Exception)
        {
            // adds to the string passed othe diagnostic info 
            if (Exception != null)
            {
                ErrorText += "\nMessage: " + Exception.Message +
                "\nType: " + Exception.GetType().ToString() +
                "\nError: " + Exception.ToString() + "\n";
                if (LoggingErrors)
                {
                    LogToFile(errorsFile, ErrorText);
                }
                if (ShowingErrors)
                {
                    Console.Out.WriteLine(ErrorText);
                }
            }
#if DEBUG
            //Get call stack
            StackTrace stackTrace = new StackTrace();
            //Log calling method name
            ErrorText += "\nMethod: " + stackTrace.GetFrame(1).GetMethod().Name;
#endif
            return ErrorText; 
        }
        /// <summary>
        /// debugging log
        /// </summary>
        /// <param name="testo">string to output</param>
        public void Debug(string testo)
        {
            if (LoggingDebug)
            {
                LogToFile(debugFile, testo);
            }
            if (ShowingDebug)
            {
                Console.Out.WriteLine(testo);
            }
        }
        public void Prompt(string testo)
        {
            Console.Out.WriteLine(testo); 
            if (LoggingPrompts)
            {
                using (StreamWriter sw = File.AppendText(promptsFile))
                {
                    sw.WriteLine(testo);
                    sw.Close();
                }
            }
        }
        private void LogToFile(string file,string testo)
        {
            try
            {
                if(!File.Exists(file))
                {
                    File.CreateText(file);
                }
                using (StreamWriter sw = File.AppendText(file))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + testo);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Couldn't save log file");
            }
        }
    }
}
