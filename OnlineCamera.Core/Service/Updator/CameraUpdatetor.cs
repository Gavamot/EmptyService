using OnlineCamera.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    /// <summary>
    /// Обновляет изображение с камер
    /// </summary>
    public class CameraUpdatetor : AbstractUpdator<Size>
    {
        readonly Camera camera;

        IGetCameraImg getCameraImg;
        Config config;
        ICameraCache cameraCache;

        public CameraUpdatetor(
            IGetCameraImg getCameraImg, 
            ICameraCache cameraCache,
            Config config)
        {
            this.getCameraImg = getCameraImg;
            this.config = config;
            this.cameraCache = cameraCache;
        }

        public override string Name => $"CameraUpdatetor {camera}";

        CancellationTokenSource source;
        public delegate void CompleteUpdate(Camera camera);
        public event CompleteUpdate CompleteUpdateEvent;

        protected int CalculateTimeForSleep(int timeMs, int fps)
        {
            int res = (1000/fps) - timeMs;
            if (res < 0)
                return 0;
            return res;
        }

        protected override async void UpdateAsync(Size parameters)
        {
            int trys = 0;
            while (trys < config.CountOfTrys)
            {
                try
                {
                    var timer = new Stopwatch();
                    timer.Start();

                    var cameraResponce = await getCameraImg.GetImgAsync(parameters, config.TimeoutMs, source);
                    this.cameraCache.SetCamera(camera, cameraResponce);

                    timer.Stop();
                    var sleepMs = CalculateTimeForSleep((int)timer.ElapsedMilliseconds, config.MaxFps);
                    if (await SleepTrueIfCanceled(1000))
                    {
                        return;
                    }
                }
                catch (CameraNotFoundException e)
                {
                    return;
                }
                catch(TimeoutException e)
                {
                    if (await SleepTrueIfCanceled(1000))
                    {
                        return;
                    }
                        
                    // Время ожидания превышенно надо попробовать снова
                    //log.Info($"getCameraImg.GetImg() timeout exception. VideoReg({camera.VideoRegIp}) is not available in this moment or ");
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
                }
                finally
                {
                    trys++;
                }
            }
        }


        // 1 Запрос к регистратору на камеры;
        // 2 Создаем потоки для камер и начинаем закачку 
        // 3 Если не получилось взять видео в течении CountOfTrys задача умирает.
        // 4 Если не вышло получить изображение спим 1 секунду и пробуем снова
    }
}
