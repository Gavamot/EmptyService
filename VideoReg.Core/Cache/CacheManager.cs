using System;
using System.Collections.Concurrent;
using System.Linq;

namespace VideoReg.Core
{
    public class CacheManager : ICache
    {
        readonly IDateService dateService;
        const int CameraLifeTimeSeconds = 5; 

        public CacheManager(IDateService dateService)
        {
            this.dateService = dateService;
            this.Cameras = new TimestamptCache<int, CameraResponce>(dateService);
        }

        readonly TimestamptCache<int, CameraResponce> Cameras;


        private int GetDiffSeconds(DateTime now, DateTime timestamp)
        {
            return (int)(now - timestamp).TotalSeconds;
        }

       
        public void UpdateVideoRegsCache()
        {
            var now = dateService.GetCurrentDateTime();
            var needRemove = Cameras.Keys.Where(x => GetDiffSeconds(now, x.ServiceTimestamp) > CameraLifeTimeSeconds);
            Cameras.RemoveAll(needRemove);
        }

        public void SetCamera(int cameraNumber, byte[] img)
        {
            var now = dateService.GetCurrentDateTime();
            Cameras.Set(cameraNumber, new CameraResponce
            {
                Img = img,
                Timestamp = now
            }, now);
        }

        /// <exception cref="KeyNotFoundException">Thrown when key not found in collection</exception>
        public CameraResponce GetCamera(int cameraNumber)
        {
            return Cameras.Get(cameraNumber);
        }
    }
}
