﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Snowrunner_Parcher.Resources
{
    internal class Logger
    {
        private static readonly string logName = string.Join("_", DateTime.Now.ToString("dd_MM_yyyy").Split(Path.GetInvalidFileNameChars()));
        private const string EXTENSION = ".log";
        public static string logPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" + Assembly.GetCallingAssembly().GetName().Name;
        public static string fullLogPath => logPath + "\\" + logName + EXTENSION;
        private static bool createdLog = false;
        private static StringBuilder logInfo = new StringBuilder();
        private static System.Timers.Timer logTimer = new System.Timers.Timer(60000);
        public static void LoadLogFile(string path = "")
        {
            if (!string.IsNullOrWhiteSpace(path)) logPath = path;

            CreateLog();
        }
        public static void AddToLog(string Log, bool forcedSave = false)
        {
            logInfo.Append(Log);
            if (forcedSave) ForceSave();

        }
        public static void AddLineLog(string info, bool flush = false)
        {
            AddToLog($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] {info} \r\n", flush);
        }
        public static void AddLineLog(string[] info, bool flush = false)
        {
            foreach (string line in info)
            {
                AddToLog($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] {line} \r\n", flush);
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
            logTimer.Elapsed += new ElapsedEventHandler(Save);

            logTimer.Start();
        }
        public static void OpenLog()
        {
            Process.Start(new ProcessStartInfo(fullLogPath)
            {
                UseShellExecute = true
            });
        }
        public static void Save(object? ob, EventArgs e)
        {
            ForceSave();
        }
        private static void ForceSave()
        {
            if (logInfo.Length == 0) return;

            File.AppendAllText(fullLogPath, logInfo.ToString());

            logInfo.Clear();
        }

    }
}
