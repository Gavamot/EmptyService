using System.Threading;
using System.Threading.Tasks;
using MoreLinq.Extensions;
using System.Collections.Concurrent;
using OnlineCamera.Core.Config.VideoRegUpdatorConfig;

namespace OnlineCamera.Core
{
    class VideoRegUpdator : AbstractUpdator
    {
        // 1 Запрашивает камеры первый раз в n секунд и ставит их на обновление 
        // 2 Второй поток пытается брать изображения с камер n раз. Если у него не получается он умирает
        public VideoRegUpdator(ILogger log, VideoRegApi api, VideoRegUpdatorConfig config) : base(log)
        {
            this.api = api;
            this.config = config;
        }

        readonly VideoRegApi api;
        readonly VideoRegUpdatorConfig config;
       
        public string Ip { get; private set; }
        public Size Size { get; private set; }

        readonly ConcurrentDictionary<int, CameraUpdatetor> CameraUpdatoros = new ConcurrentDictionary<int, CameraUpdatetor>();
        readonly CancellationTokenSource source = new CancellationTokenSource();

        public override string Name => ToString();
        public override string ToString() => $"VideoRegUpdator({Ip} {Size})";

        bool AddNewCameraUpdator(int camNum)
        {
            if (CameraUpdatoros.ContainsKey(camNum))
                return false;
            var camUpdator = new CameraUpdatetor(log, api, config);
            camUpdator.CompleteUpdateEvent += (cam) => CameraUpdatoros.Remove(cam.Number);
            bool res = CameraUpdatoros.AddIfNotExist(camNum, camUpdator);
            camUpdator.Start(source);
            return res;
        }

        protected override async void UpdateAsync()
        {
            while (source.IsCancellationRequested)
            {
                var cameras = await api.GetCameras(source);
                cameras.ForEach(camNum => AddNewCameraUpdator(camNum));
                try
                {
                    await Task.Delay(config.GetCamerasReqvestIntervalMs, source.Token);
                }
                catch (TaskCanceledException e)
                {
                    break;
                }
            }
        }
    }
}
