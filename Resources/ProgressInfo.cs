using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowrunner_Parcher.Resources
{
    public sealed class ProgressInfo
    {
        public string Info { get; set; }
        private int progress = 0;
        public int Patched
        {
            get
            {
                if (!Info.Contains('/')) return -1;

                return int.Parse(Info.Split('/')[0]);
            }
        }
        public int Total
        {
            get
            {
                if (!Info.Contains('/')) return -1;

                return int.Parse(Info.Split('/')[1]);
            }
        }
        public int percentatge
        {
            get
            {
                if (progress >= (Patched * 100) / Total)
                    return progress;

                progress = (Patched * 100) / Total;
                return progress;
            }
        }
        public ProgressInfo(string info = "")
        {
            Info = info;
        }

    }
}
