using System;

namespace OnlineCamera.Core
{
    public class CameraResponce
    {
        public CameraResponce() { }
        public DateTime TimeStamp { get; set; }
        public VideoRegInfo Info { get; set; }
        public byte[] Image { get; set; }
        public override string ToString() => $"{Info}  [img.length = {Image?.Length}]) - {TimeStamp.ToString("dd/MM/yyyy HH:mm:ss")}";
    }
}
