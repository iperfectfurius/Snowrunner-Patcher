using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Snowrunner_Patcher.Resources
{
    internal class Logger
    {
        private static readonly string logName = string.Join("_", DateTime.Now.ToString("yyyyMMdd").Split(Path.GetInvalidFileNameChars()));
        private const string EXTENSION = ".log";
        public static string logPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" + Assembly.GetCallingAssembly().GetName().Name;
        public static string fullLogPath => logPath + "\\" + logName + EXTENSION;
        private static bool createdLog = false;
        private static StringBuilder logInfo = new StringBuilder();
        private static System.Timers.Timer logTimer = new System.Timers.Timer(60000);
        public enum OpenParam : byte
        {
            LogFile,
            LogFolder
        }
        public static void LoadLogFile(string path = "")
        {
            if (!string.IsNullOrWhiteSpace(path)) logPath = path;

            CreateLog();
        }
        public static void AddToLog(string Log, bool forcedSave = false)
        {
            logInfo.Append(Log + "\r\n");
            if (forcedSave) ForceSave();
        }
        public static void AddToLog(string[] Log, bool forcedSave = false)
        {
            foreach (string line in Log)
            {
                logInfo.Append(line + "\r\n");
            }              
            if (forcedSave) ForceSave();
        }
        public static void AddLineLog(string info, bool flush = false)
        {
            AddToLog($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] {info}", flush);
        }
        public static void AddLineLog(string[] info, bool flush = false)
        {
            foreach (string line in info)
            {
                AddToLog($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] {line}", flush);
            }
        }
        private static void CreateLog()
        {
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            if (!File.Exists(fullLogPath))
                File.Create(fullLogPath).Close();

            createdLog = true;

            logTimer.AutoReset = true;
            logTimer.Elapsed += new ElapsedEventHandler(SaveLog);

            logTimer.Start();
        }
        public static void Open(OpenParam ToOpen)
        {
            string PathParam = "";

            switch (ToOpen)
            {
                case OpenParam.LogFile:
                    PathParam = fullLogPath;
                    break;
                case OpenParam.LogFolder:
                    PathParam = logPath;
                    break;
            }
            ForceSave();
            Process.Start(new ProcessStartInfo(PathParam)
            {
                UseShellExecute = true
            });
        }
        public static void SaveLog(object? ob, EventArgs? e)
        {
            ForceSave();
        }
        private static void ForceSave()
        {
            if (logInfo.Length == 0) return;
            try
            {
                File.AppendAllText(fullLogPath, logInfo.ToString());
                logInfo.Clear();
            }
            catch (Exception ex)
            {
                AddLineLog($"[ERROR] Error trying to save the LOG. {ex.Message}");
            }           
        }
    }
}
