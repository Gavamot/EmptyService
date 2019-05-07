using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCamera.Value
{
    public class VideoReg
    {
        public string Ip { get; set; }
        public VideoRegInfo Info { get; set; }
        public CameraInfo[] Cameras { get; set; } = new CameraInfo[0];
    }
}
