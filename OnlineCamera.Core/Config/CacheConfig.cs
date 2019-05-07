using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCamera.Core.Config
{
    public class CacheConfig
    {
        public int VideoRegLifeTimeSeconds { get; set; } = 3600;
        public int CameraLifeTimeSeconds { get; set; } = 900;
    }
}
