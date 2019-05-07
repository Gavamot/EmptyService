using OnlineCamera.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCamera.Service
{
    public interface IDateService
    {
        public DateTime GetCurrentDateTime();
    }

    public class DateService : IDateService
    {
        public DateTime GetCurrentDateTime() => DateTime.Now;
        
    }

    public class CameraCache
    {
        readonly IDateService dateService; 
        public CameraCache(IDateService dateService)
        {
            this.dateService = dateService;
        }

        private Dictionary<CameraResponce, byte[]> cache = new Dictionary<CameraResponce, byte[]>();

        /// <summary>
        /// You can initiate cache before do work. (For tests)
        /// </summary>
        public void SetCasche(Dictionary<CameraResponce, byte[]> cache)
        {
            this.cache = cache;
        }

        public byte[] GetImage(string ip, int cameraNumber) 
        {
            var key = new CameraResponce(ip, cameraNumber);
            byte[] res;
            cache.TryGetValue(key, out res);
            return res;
        }

        public VideoReg[] GetCameras()
        {
            var keys = cache.Keys.ToArray();
            var keyValue = keys.GroupBy(x => x.Camera.VideoRegIp).ToArray();
            var res = new VideoReg[keyValue.Length];

            var now = dateService.GetCurrentDateTime();

            for (int i = 0; i < keyValue.Length; i++)
            {
                var reg = keyValue[i].First();
                res[i] = new VideoReg
                {
                    BrigadeCode = reg.Info.BrigadeCode,
                    Ip = reg.Camera.VideoRegIp,
                    IveSerial = reg.Info.IveSerial,
                    VideoRegSerial = reg.Info.VideoRegSerial,
                    Version = reg.Info.Version,
                    Cameras = keyValue[i]
                    .Select(x => new CameraInfo {
                        Number = x.Camera.Number,
                        OldSecond = (int)(now - x.TimeStamp).TotalSeconds })
                    .OrderBy(x => x.Number)
                    .ToArray()
                };
            }
            return res;
        }


    }
}
