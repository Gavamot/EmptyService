using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    public class Config
    {
        public string IpFile { get; set; }
        public int VideoRegPollingIntervalSeconds { get; set; }
        public int CountOfTrys { get; set; }
        public int MaxFps { get; set; }
        public int TimeoutMs { get; set; }
        public int VideoRegLifeTimeSeconds { get; set; }
        public int CameraLifeTimeSeconds { get; set; }
    }
}
