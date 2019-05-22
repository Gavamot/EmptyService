using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoReg.Core
{
    public class VideoRegUpdator : AbstractUpdator<object>
    {
        const int IterationSleepMs = 1000;

        readonly ICamerasRep camerasRep;
        readonly IImgRep imgRep;
        readonly ICache cache;

        // 1 Запрашивает камеры первый раз в n секунд и ставит их на обновление 
        // 2 Второй поток пытается брать изображения с камер n раз. Если у него не получается он умирает
        public VideoRegUpdator(
            ICamerasRep camerasRep,
            IImgRep imgRep,
            ICache cache,
            IAppLogger log) : base(log)
        {
            this.cache = cache;
            this.imgRep = imgRep;
            this.camerasRep = camerasRep;
        }

        readonly ConcurrentDictionary<int, CameraUpdatetor> CameraUpdatoros = new ConcurrentDictionary<int, CameraUpdatetor>();

        public override string Name => ToString();
        public override string ToString() => $"VideoRegUpdator";

        bool AddNewCameraUpdator(CameraSettings cameraSettings)
        {
            if (CameraUpdatoros.ContainsKey(cameraSettings.Number))
                return false;
            
            var camUpdator = new CameraUpdatetor(imgRep, cache, log);
            camUpdator.OnCompleteUpdate += (updator ,cam) => CameraUpdatoros.Remove(cam.Number);
            bool res = CameraUpdatoros.AddIfNotExist(cameraSettings.Number, camUpdator);
            camUpdator.Start(cameraSettings);
            return res;
        }

        protected override async Task UpdateAsync(object parameters)
        {
            while (true)
            {
                try
                {
                    var cameras = await camerasRep.GetCameraSettingsAsync();
                    foreach(var cam in cameras)
                    {
                        AddNewCameraUpdator(cam);
                    }
                    log.Debug($"{Name} got info [{string.Join<CameraSettings>(" | ", cameras)}]");
                }
                catch (TimeoutException e)
                {
                    log.Error($"{Name} TimeoutException ({e.Message})");
                    
                }
                catch (Exception e)
                {
                    log.Error($"{Name} update error ({e.Message})", e);
                }

                await Task.Delay(IterationSleepMs);
            }
        }
       
    }
}
