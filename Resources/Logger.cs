using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snowrunner_Parcher.Resources
{
    static class Logger
    {
        private static readonly string logName = string.Join("_", DateTime.Now.ToString("dd_MM_yyyy").Split(Path.GetInvalidFileNameChars()));
        private const string EXTENSION = ".log";
        public static string logPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" +Assembly.GetCallingAssembly().GetName().Name;
        public static string fullLogPath => logPath + "\\" + logName + EXTENSION;
        private static FileStream? LogFile = null;
        public static void LoadLogFile(string path = "")
        {
            if (!string.IsNullOrWhiteSpace(path)) logPath = path;

            CreateLog();
        }
        public static bool WriteLog(string Log)
        {
            LogFile?.Write(Encoding.ASCII.GetBytes(Log));
            LogFile?.Flush();
            return true;
        }
        private static void CreateLog()
        {
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            if (LogFile != null) LogFile.Close();

            LogFile = File.Create(fullLogPath);
        }

    }
}
