using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCamera.Value
{
    public class CameraResponce
    {
        public CameraResponce() { }
        public CameraResponce(string ip, int num)
        {
            Camera = new Camera
            {
                VideoRegIp = ip,
                Number = num
            };
        }

        public Camera Camera { get; set; }
        public DateTime TimeStamp { get; set; }
        public Info Info { get; set; }

        public override bool Equals(object obj)
        {
            var cam = obj as CameraResponce;
            if (cam == null) return false;
            return Camera.Equals(cam.Camera);
        }

        public override int GetHashCode()
        {
            return Camera.GetHashCode();
        }

        public override string ToString() => $"{Camera.ToString()} - {TimeStamp.ToString("dd/MM/yyyy HH:mm:ss")}";
    }

    public class Camera
    {
        public string VideoRegIp { get; set; }
        public int Number { get; set; }

        public override bool Equals(object obj)
        {
            var cam = obj as Camera;
            if (cam == null) return false;
            if (GetHashCode() != cam.GetHashCode()) return false;
            return VideoRegIp == cam.VideoRegIp && Number == cam.Number; 
        }

        public override int GetHashCode()
        {
            const int HashingBase = unchecked((int)  2166136261);
            const int HashingMultiplier = 16777619;

            int hash = HashingBase;
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, VideoRegIp) ? VideoRegIp.GetHashCode() : 0);
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, Number) ? Number.GetHashCode() : 0);
            return hash;
        }

        public override string ToString() => $"[{VideoRegIp}] cam={Number}";
    }

    public class Info
    {
        public int BrigadeCode { get; set; }
        public string VideoRegSerial { get; set; }
        public string IveSerial { get; set; }
        public string Version { get; set; }
    }
}
