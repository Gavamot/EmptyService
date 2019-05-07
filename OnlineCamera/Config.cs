using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCamera
{
    public class Config
    {
        public string IpFile { get; set; }
        public int IvePollIntervalSecond { get; set; }
        public int MaxFps { get; set; }
        public int CameraLifeTimeSecond { get; set; }
    }
}
