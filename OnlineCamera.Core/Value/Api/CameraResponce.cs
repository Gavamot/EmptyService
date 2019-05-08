using System;

namespace OnlineCamera.Core
{
    public class CameraResponce
    {
        public DateTime SourceTimestamp { get; set; }
        public byte[] Img { get; set; }
        public override string ToString() => $"[img.length = {Img?.Length}]) - {SourceTimestamp.ToString("dd/MM/yyyy HH:mm:ss")}";
    }
}
