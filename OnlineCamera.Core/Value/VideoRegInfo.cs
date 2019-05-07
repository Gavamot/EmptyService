using System;
using System.Collections.Generic;

namespace OnlineCamera.Core
{
    public class VideoRegInfo
    {
        public string Ip { get; set; }
        public int BrigadeCode { get; set; }
        public string VideoRegSerial { get; set; }
        public string IveSerial { get; set; }
        public string Version { get; set; }
        public int LastAgoSeconds { get; set; }
    }
}
