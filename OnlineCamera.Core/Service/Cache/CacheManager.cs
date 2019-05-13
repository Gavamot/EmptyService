using OnlineCamera.Core;
using System;
using System.Collections.Concurrent;
using System.Linq;
using MoreLinq;

namespace OnlineCamera.Core
{
    public interface IVideoRegInfoCache
    {
        void SetVideoRegInfo(string ip, VideoRegInfo info, DateTime sourceTimestamp);
    }

    public interface ICameraCache
    {
        void SetCamera(Camera camera, CameraResponce snapshot);
    }

    public interface IVideoRegsCache
    {
        VideoReg[] VideoRegs { get; }
    }

    public interface ICache : IVideoRegInfoCache, ICameraCache, IVideoRegsCache { };

    public class CacheManager : ICache
    {
        readonly IDateService dateService;
        readonly Config config;
        public CacheManager(IDateService dateService, Config config)
        {
            this.dateService = dateService;
            this.VideoRegInfo = new TimestamptCache<string, VideoRegInfo>(dateService);
            this.Cameras = new TimestamptCache<Camera, Snapshot>(dateService);
            this.config = config;
        }

        readonly TimestamptCache<string, VideoRegInfo> VideoRegInfo;
        readonly TimestamptCache<Camera, Snapshot> Cameras;

        volatile VideoReg[] videoRegs = new VideoReg[0];
        public VideoReg[] VideoRegs => videoRegs;

        private int GetDiffSeconds(DateTime now, DateTime timestamp)
        {
            return (int)(now - timestamp).TotalSeconds;
        }

        private void RemoveOld<Tkey, Tvelue>(TimestamptCache<Tkey, Tvelue> cache, DateTime now, int lifeTimeSeconds)
        {
            var needRemove = cache.Keys.Where(x => GetDiffSeconds(now, x.ServiceTimestamp) > lifeTimeSeconds);
            cache.RemoveAll(needRemove);
        }

        private void RemoveOldItems(DateTime now)
        {
            RemoveOld(VideoRegInfo, now, config.VideoRegLifeTimeSeconds);
            RemoveOld(Cameras, now, config.CameraLifeTimeSeconds);
        }

        public void UpdateVideoRegsCache()
        {
            var now = dateService.GetCurrentDateTime();
            RemoveOldItems(now);

            var cameras = Cameras.Keys.ToArray()
                .Select(x => new CameraInfo
                {
                    Number = x.Key.Number,
                    LastSnapshotAgoSeconds = GetDiffSeconds(now, x.ServiceTimestamp)
                })
                .ToArray();

            var videoRegs = VideoRegInfo.ToArray()
                .Select(x => new VideoReg
                {
                    Info = new VideoRegInfo
                    {
                        LastAgoSeconds = GetDiffSeconds(now, x.Key.ServiceTimestamp),
                        Ip = x.Key.Key,
                        BrigadeCode = x.Value.BrigadeCode,
                        IveSerial = x.Value.IveSerial,
                        Version = x.Value.Version,
                        VideoRegSerial = x.Value.VideoRegSerial
                    },
                    Cameras = cameras.Where(c => c.VideoRegIp == x.Key.Key).ToArray()
                });
        }

        public void SetVideoRegInfo(string ip, VideoRegInfo info, DateTime sourceTimestamp) =>
            VideoRegInfo.Set(ip, info, sourceTimestamp);

        public void SetCamera(Camera camera, CameraResponce responce)
            => Cameras.Set(camera, 
                new Snapshot {
                    Camera = camera,
                    Img = responce.Img,
                    SourceTimestamp = responce.SourceTimestamp },
                    responce.SourceTimestamp
                );
    }
}
