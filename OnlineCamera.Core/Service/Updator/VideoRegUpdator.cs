using System.Threading;
using System.Threading.Tasks;
using MoreLinq.Extensions;
using System.Collections.Concurrent;
using System;

namespace OnlineCamera.Core
{
    class VideoRegUpdator : AbstractUpdator<VideoRegReqvestSettings>
    {
        // 1 Запрашивает камеры первый раз в n секунд и ставит их на обновление 
        // 2 Второй поток пытается брать изображения с камер n раз. Если у него не получается он умирает
        public VideoRegUpdator(IVideoRegApi api,
            ICache cache,
            IStatistRegistrator statisticRegistrator,
            Config config)
        {
            this.api = api;
            this.config = config;
            this.cache = cache;
        }

        readonly IStatistRegistrator statisticRegistrator;
        readonly ICache cache;
        readonly IVideoRegApi api;
        readonly Config config;
       
        public string Ip { get; private set; }

        readonly ConcurrentDictionary<int, CameraUpdatetor> CameraUpdatoros = new ConcurrentDictionary<int, CameraUpdatetor>();
        readonly CancellationTokenSource source = new CancellationTokenSource();

        public override string Name => ToString();
        public override string ToString() => $"VideoRegUpdator({Ip})";

        bool AddNewCameraUpdator(int camNum, Size size)
        {
            if (CameraUpdatoros.ContainsKey(camNum))
                return false;
            var camUpdator = new CameraUpdatetor(api, cache, config);
            camUpdator.CompleteUpdateEvent += (cam) => CameraUpdatoros.Remove(cam.Number);
            bool res = CameraUpdatoros.AddIfNotExist(camNum, camUpdator);
            camUpdator.Start(size, source);
            return res;
        }

        protected override async void UpdateAsync(VideoRegReqvestSettings parameters)
        {
            while (source.IsCancellationRequested)
            {
                try
                {
                    var regResponce = await api.GetVideoRegInfoAsync(config.TimeoutMs, source);
                    cache.SetVideoRegInfo(Ip, new VideoRegInfo
                    {
                        Ip = Ip,
                        BrigadeCode = regResponce.BrigadeCode,
                        IveSerial = regResponce.IveSerial,
                        Version = regResponce.Version,
                        VideoRegSerial = regResponce.VideoRegSerial
                    }, regResponce.CurrentDate);
                    regResponce.Cameras.ForEach(camNum => AddNewCameraUpdator(camNum, parameters.Size));

                    statisticRegistrator.RegAsync(Ip, regResponce).Start();

                    if (await SleepTrueIfCanceled(config.VideoRegPollingIntervalSeconds))
                    {
                        return;
                    }
                }
                catch (TimeoutException e)
                {
                    if (await SleepTrueIfCanceled(1000))
                    {
                        return;
                    }
                    // Время ожидания превышенно надо попробовать снова
                    //log.Info($"api.GetVideoRegInfo() timeout exception. VideoReg({Ip}) is not available in this moment or ");
                }
                catch (OperationCanceledException e)
                {
                    return;
                }
                catch (Exception e)
                {
                    if (await SleepTrueIfCanceled(1000))
                    {
                        return;
                    }
                    // Неизвестная ошибка надо попробывать снова
                    //log.Info(e.Message);
                }
            }
        }
    }
}
