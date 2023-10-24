using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snowrunner_Parcher.Resources
{
    internal class Logger
    {
        public static readonly string Extension = ".log";
        public static string Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" +Assembly.GetCallingAssembly().GetName().Name;

        public static bool WriteLog(string Log,string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = Path;
         
            
            return true;
        }
    }
}
