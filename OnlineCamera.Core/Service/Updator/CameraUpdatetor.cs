using System;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    /// <summary>
    /// Обновляет изображение с камер
    /// </summary>
    public class CameraUpdatetor : AbstractUpdator<Camera>
    {
        const int SleepIfErrorMs = 200;

        Camera camera;
        IGetCameraImg getCameraImg;
        Config config;
        ICameraCache cameraCache;

        public CameraUpdatetor(
            IGetCameraImg getCameraImg, 
            ICameraCache cameraCache,
            Config config,
            IAppLogger log) : base(log)
        {
            this.getCameraImg = getCameraImg;
            this.config = config;
            this.cameraCache = cameraCache;
        }

        public override string Name => $"CameraUpdatetor {camera}";

        public event Action<CameraUpdatetor, Camera> OnCompleteUpdate;

        // 1 Запрос к регистратору на камеры;
        // 2 Создаем потоки для камер и начинаем закачку 
        // 3 Если не получилось взять видео в течении CountOfTrys задача умирает.
        // 4 Если не вышло получить изображение спим 1 секунду и пробуем снова
        protected int CalculateTimeForSleep(int timeMs, int fps)
        {
            int res = (1000/fps) - timeMs;
            if (res < 0)
                return 0;
            return res;
        }
   
        protected override async Task UpdateAsync(Camera parameters)
        {
            this.camera = parameters;
            int trys = 0;
            var timeStampt = DateTime.MinValue; 
            while (trys < config.CountOfTrys)
            {
                try
                {
                    var timer = new Stopwatch();
                    timer.Start();
                    var cameraResponce = await getCameraImg.GetImgAsync(camera, timeStampt, config.TimeoutMs, source);
                    timer.Stop();

                    if (cameraResponce == null) // Дата изображения на сервере не изменилась
                    {
                        log.Warning($" {Name} img was repeted ... timespampt={timeStampt.ToShortTimeString() }");
                        if (await SleepTrueIfCanceled(SleepIfErrorMs))
                        {
                            return;
                        }
                        continue;
                    }

                    timeStampt = cameraResponce.SourceTimestamp;
                    this.cameraCache.SetCamera(camera, cameraResponce);
                    var sleepMs = CalculateTimeForSleep((int) timer.ElapsedMilliseconds, config.MaxFps);
                    log.Debug($"{Name} {parameters} - got {cameraResponce}  time execution = {timer.ElapsedMilliseconds} ms | time for sleep = {sleepMs} ms");

                    if (await SleepTrueIfCanceled(sleepMs))
                    {
                        return;
                    }
                }
                catch (CameraNotFoundException)
                {
                    log.Warning($"{Name} camera {camera} have no connect is lost");
                    return;
                }
                catch(TimeoutException e)
                {
                    log.Error($"{Name} TimeoutException ({e.Message})");
                    if (await SleepTrueIfCanceled(SleepIfErrorMs))
                    {
                        return;
                    }
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception e)
                {
                    log.Error($"{Name} update error ({e.Message})", e);
                    if (await SleepTrueIfCanceled(SleepIfErrorMs))
                    {
                        return;
                    }
                    // Неизвестная ошибка надо попробывать снова
                    trys++;
                }
            }
        }

        protected override void OnComplete()
        {
            OnCompleteUpdate(this, camera);
        }
    }
}
