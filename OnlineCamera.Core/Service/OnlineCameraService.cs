using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    public class OnlineCameraService : IOnlineCameraService
    {
        readonly Dictionary<string, VideoRegUpdator> videoRegUpdators = new Dictionary<string, VideoRegUpdator>();
        readonly IVideoRegApi api;
        readonly ICache cache;
        readonly Config config;
        readonly IStatistRegistrator statistRegistrator;
        private readonly object objLock = new object();
        readonly IAppLogger log;
        public OnlineCameraService(IVideoRegApi api, ICache cache, IStatistRegistrator statistRegistrator, Config config, IAppLogger log)
        {
            this.api = api;
            this.cache = cache;
            this.config = config;
            this.statistRegistrator = statistRegistrator;
            this.log = log;
        }

        public void AddVideoReg(VideoRegReqvestSettings settings)
        {
            lock (objLock)
            {
                if (videoRegUpdators.ContainsKey(settings.Ip))
                    return;
                var videoRegUpdator = new VideoRegUpdator(api, cache, statistRegistrator, config, log);
                videoRegUpdators.Add(settings.Ip, videoRegUpdator);
                videoRegUpdator.Start(settings, new CancellationTokenSource());
            }
        }

        public void AddVideoRegs(IEnumerable<VideoRegReqvestSettings> settings)
        {
            foreach (var s in settings)
            {
                AddVideoReg(s);
            }
        }
    }
}
