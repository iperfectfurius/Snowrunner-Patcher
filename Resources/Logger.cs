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
        private static readonly string logName = string.Join("_", DateTime.Now.ToString().Split(Path.GetInvalidFileNameChars()));
        private const string EXTENSION = ".log";
        public static string logPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" +Assembly.GetCallingAssembly().GetName().Name;
        public static string fullLogPath => logPath + "\\" + logName + EXTENSION;
        public static bool WriteLog(string Log,string path = null)
        {

            if (!string.IsNullOrWhiteSpace(path))
                logPath = path;

            if (!File.Exists(fullLogPath))
                CreateLog();

            File.AppendAllText(fullLogPath, Log);

            return true;
        }

        private static void CreateLog()
        {
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            File.Create(fullLogPath).Close();

        }
    }
}
