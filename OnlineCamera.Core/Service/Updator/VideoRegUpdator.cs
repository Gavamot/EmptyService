using System.Threading;
using System.Threading.Tasks;
using MoreLinq.Extensions;
using System.Collections.Concurrent;
using OnlineCamera.Core.Config.VideoRegUpdatorConfig;
using System;

namespace OnlineCamera.Core
{
    class VideoRegUpdator : AbstractUpdator
    {
        // 1 Запрашивает камеры первый раз в n секунд и ставит их на обновление 
        // 2 Второй поток пытается брать изображения с камер n раз. Если у него не получается он умирает
        public VideoRegUpdator(ILogger log, IVideoRegApi api, ICache cache, VideoRegUpdatorConfig config) : base(log)
        {
            this.api = api;
            this.config = config;
            this.cache = cache;
        }

        readonly ICache cache;
        readonly IVideoRegApi api;
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
            var camUpdator = new CameraUpdatetor(log, api, config, cache);
            camUpdator.CompleteUpdateEvent += (cam) => CameraUpdatoros.Remove(cam.Number);
            bool res = CameraUpdatoros.AddIfNotExist(camNum, camUpdator);
            camUpdator.Start(source);
            return res;
        }

        protected override async void UpdateAsync()
        {
            while (source.IsCancellationRequested)
            {
                try
                {
                    var regResponce = await api.GetVideoRegInfoAsync(source);
                    cache.SetVideoRegInfo(Ip, new VideoRegInfo
                    {
                        Ip = Ip,
                        BrigadeCode = regResponce.BrigadeCode,
                        IveSerial = regResponce.IveSerial,
                        Version = regResponce.Version,
                        VideoRegSerial = regResponce.VideoRegSerial
                    }, regResponce.CurrentDate);
                    regResponce.Cameras.ForEach(camNum => AddNewCameraUpdator(camNum));
                }
                catch (TimeoutException e)
                {
                    // Время ожидания превышенно надо попробовать снова
                    log.Info($"api.GetVideoRegInfo() timeout exception. VideoReg({Ip}) is not available in this moment or ");
                }
                catch (OperationCanceledException e)
                {
                    return;
                }
                catch (Exception e)
                {
                    // Неизвестная ошибка надо попробывать снова
                    log.Info(e.Message);
                }

                try
                {
                    await Task.Delay(config.GetVideoRegInfoIntervalMs, source.Token);
                }
                catch(OperationCanceledException e)
                {
                    return;
                }
            }
        }
    }
}
