namespace OnlineCamera.Core
{
    public class Camera
    {
        public Camera() { }
        public static Camera CreateCamera(VideoRegReqvestSettings settings, int number)
        {
            return new Camera { Settings = settings, Number = number };
        }

        public static readonly Camera EmptyCamera = new Camera();

        public VideoRegReqvestSettings Settings { get; set; } = new VideoRegReqvestSettings();
        public int Number { get; set; }
        public override bool Equals(object obj)
        {
            var cam = obj as Camera;
            if (cam == null) return false;
            if (GetHashCode() != cam.GetHashCode()) return false;
            return Settings?.Ip == cam.Settings?.Ip && Number == cam.Number; 
        }

        public override int GetHashCode()
        {
            const int HashingBase = unchecked((int)  2166136261);
            const int HashingMultiplier = 16777619;

            int hash = HashingBase;
            if(Settings != null )
            {
                hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, Settings.Ip) ? Settings.Ip.GetHashCode() : 0);
            }
            hash = (hash * HashingMultiplier) ^ (!object.ReferenceEquals(null, Number) ? Number.GetHashCode() : 0);
            return hash;
        }

        public override string ToString() => $"[{Settings?.Ip}] cam={Number} size={Settings?.Size}";
    }
}
