using System.Threading;
using System.Threading.Tasks;
using MoreLinq.Extensions;
using System.Collections.Concurrent;
using System;
using System.Net;
using OnlineCamera.Core.Value.Api;

namespace OnlineCamera.Core
{
    class VideoRegUpdator : AbstractUpdator<VideoRegReqvestSettings>
    {
        // 1 Запрашивает камеры первый раз в n секунд и ставит их на обновление 
        // 2 Второй поток пытается брать изображения с камер n раз. Если у него не получается он умирает
        public VideoRegUpdator(IVideoRegApi api,
            ICache cache,
            IStatistRegistrator statisticRegistrator,
            Config config,
            IAppLogger log) : base(log)
        {
            this.api = api;
            this.config = config;
            this.cache = cache;
        }

        readonly IStatistRegistrator statisticRegistrator;
        readonly ICache cache;
        readonly IVideoRegApi api;
        readonly Config config;

        VideoRegReqvestSettings parameters;
        string Ip => parameters?.Ip;

        readonly ConcurrentDictionary<int, CameraUpdatetor> CameraUpdatoros = new ConcurrentDictionary<int, CameraUpdatetor>();
        readonly CancellationTokenSource source = new CancellationTokenSource();

        public override string Name => ToString();
        public override string ToString() => $"VideoRegUpdator({Ip})";

        bool AddNewCameraUpdator(Camera camera)
        {
            if (CameraUpdatoros.ContainsKey(camera.Number))
                return false;
            
            var camUpdator = new CameraUpdatetor(api, cache, config, log);
            camUpdator.OnCompleteUpdate += (updator ,cam) => CameraUpdatoros.Remove(cam.Number);
            bool res = CameraUpdatoros.AddIfNotExist(camera.Number, camUpdator);
            camUpdator.Start(camera, source);
            return res;
        }

        protected override async Task UpdateAsync(VideoRegReqvestSettings parameters)
        {
            this.parameters = parameters;
            while (!source.IsCancellationRequested)
            {
                VideoRegResponce regResponce = null;
                try
                {
                    regResponce = await api.GetVideoRegInfoAsync(Ip, config.TimeoutMs, source);
                }
                catch (TimeoutException e)
                {
                    log.Error($"{Name} TimeoutException ({e.Message})");
                }
                catch (OperationCanceledException e)
                {
                    return;
                }
                catch(WebException e)
                {
                    log.Error($"{Name} update error ({e.Message})", e);
                }

                if(regResponce == null) 
                {
                    if (await SleepTrueIfCanceled(1000))
                    {
                        return;
                    }
                    continue;
                }

                cache.SetVideoRegInfo(Ip, new VideoRegInfo
                {
                    Ip = Ip,
                    BrigadeCode = regResponce.BrigadeCode,
                    IveSerial = regResponce.IveSerial,
                    Version = regResponce.Version,
                    VideoRegSerial = regResponce.VideoRegSerial
                }, regResponce.CurrentDate);

                regResponce.Cameras.ForEach(
                    camNum => AddNewCameraUpdator(
                        Camera.CreateCamera(Ip, camNum, parameters.Size, parameters.Quality)
                    )
                );

                log.Debug($"{Name} got info [{regResponce}]");

                // Запускаем процедуру задачу сбора статистики
                // statisticRegistrator.RegAsync(Ip, regResponce).Start();

                if (await SleepTrueIfCanceled(config.VideoRegPollingIntervalSeconds))
                {
                    return;
                }
            }
        }
    }
}
