using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{ 
    public class Snapshot
    {
        public DateTime SourceTimestamp { get; set; }
        public byte[] Img { get; set; }
        public Camera Camera { get; set; }
        public int LastInfoUpdateAgoSeconds { get; set; }
    }
}
