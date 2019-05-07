using OnlineCamera.Core;
using OnlineCamera.Value;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace OnlineCamera.Service
{
    public class CacheManager
    {
        readonly IDateService dateService;
        public CacheManager(IDateService dateService, Ca)
        {
            this.dateService = dateService;
            this.VideoRegInfo = new TimestamptCache<string, VideoRegInfo>(dateService);
            this.Cameras = new TimestamptCache<Camera, byte[]>(dateService);
        }

        public readonly TimestamptCache<string, VideoRegInfo> VideoRegInfo;
        public readonly TimestamptCache<Camera, Snapshot > Cameras;

        volatile VideoReg[] videoRegs = new VideoReg[0];
        public VideoReg[] VideoRegs => videoRegs;

        void UpdateVideoRegs()
        {
            DateTime now = 
            var videoRegs = VideoRegInfo.cache.ToArray();
            var cameras = Cameras.cache.Keys.ToArray();

            videoRegs.
        }
    }
}
