using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snowrunner_Parcher.Resources
{
    internal class Logger
    {
        private static readonly string logName = string.Join("_", DateTime.Now.ToString("dd_MM_yyyy").Split(Path.GetInvalidFileNameChars()));
        private const string EXTENSION = ".log";
        public static string logPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" +Assembly.GetCallingAssembly().GetName().Name;
        public static string fullLogPath => logPath + "\\" + logName + EXTENSION;
        //private static FileStream? LogFile = null;
        private static bool createdLog = false;
        private static StringBuilder logInfo = new StringBuilder();
        //private static Timer logTimer = new Timer();
        public static void LoadLogFile(string path = "")
        {
            if (!string.IsNullOrWhiteSpace(path)) logPath = path;

            CreateLog();
        }
        public static void AddToLog(string Log, bool forcedSave = false)
        {
            //if (LogFile == null) LoadLogFile();

            ////LogFile?.Write(Encoding.ASCII.GetBytes(Log));
            //if (flush) LogFile?.Flush();


        }
        public static void AddLineLog(string info, bool flush = false)
        {
            AddToLog($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] {info} \r\n",flush);
        }
        private static void CreateLog()
        {
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            File.Create(logPath).Close();
            createdLog = true;
        }
        public static void OpenLog()
        {
            Process.Start(new ProcessStartInfo(fullLogPath)
            {
                UseShellExecute = true
            });
        }
        public static void Save(dynamic _,dynamic __)
        {
            ForceSave();
        }
        private static void ForceSave()
        {
            File.AppendAllText(fullLogPath,logInfo.ToString());
        }

    }
}
