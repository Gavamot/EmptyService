using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCamera.Core.Value.Api
{
    public class VideoRegResponce
    {
        public DateTime CurrentDate { get; set; }
        public int BrigadeCode { get; set; }
        public string VideoRegSerial { get; set; }
        public string IveSerial { get; set; }
        public string Version { get; set; }
        public int [] Cameras { get; set; } 
    }
}
