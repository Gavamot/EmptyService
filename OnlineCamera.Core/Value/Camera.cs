namespace OnlineCamera.Core
{
    public class Camera
    {
        public Camera() { }
        public Camera(string videoRegIp, int number)
        {
            VideoRegIp = videoRegIp;
            Number = number;
        }

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
}
