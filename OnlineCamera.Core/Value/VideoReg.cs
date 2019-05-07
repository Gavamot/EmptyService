using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    public class VideoReg
    {
        public VideoRegInfo Info { get; set; }
        public CameraInfo[] Cameras { get; set; } = new CameraInfo[0];
    }
}
