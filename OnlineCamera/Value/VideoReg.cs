using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCamera.Value
{
    public class VideoReg
    {
        public string Ip { get; set; }
        public int BrigadeCode { get; set; }
        public string VideoRegSerial { get; set; }
        public string IveSerial { get; set; }
        public string Version { get; set; }
        public CameraInfo[] Cameras { get; set; } = new CameraInfo[0];
    }

    public class CameraInfo
    {
        public int Number { get; set; }
        public int OldSecond { get; set; }
    }
}
