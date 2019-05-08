using OnlineCamera.Core;
using OnlineCamera.Core.Config.VideoRegUpdatorConfig;
using System;
using System.Collections.Generic;
using System.Threading;

namespace OnlineCamera.Core
{
    public class CameraUpdatetor : AbstractUpdator
    {
        readonly Camera camera;
        readonly Size size;

        IGetCameraImg getCameraImg;
        VideoRegUpdatorConfig config;
        ICameraCache cameraCache;

        public CameraUpdatetor(ILogger log, 
            IGetCameraImg getCameraImg, 
            VideoRegUpdatorConfig config,
            ICameraCache cameraCache) : base(log)
        {
            this.getCameraImg = getCameraImg;
            this.config = config;
            this.cameraCache = cameraCache;
        }

        public override string Name => $"CameraUpdatetor {camera} {size}";

        CancellationTokenSource source;
        public delegate void CompleteUpdate(Camera camera);
        public event CompleteUpdate CompleteUpdateEvent;
        protected override async void UpdateAsync()
        {
            int trys = 0;
            while (trys < config.CountOfTrys)
            {
                try
                {
                    var cameraResponce = await getCameraImg.GetImgAsync(size, config.DelayMs, source);
                    this.cameraCache.SetCamera(camera, cameraResponce);
                }
                catch (CameraNotFoundException e)
                {
                    // Видеорегистратор не имеет камеры значит необходимо нет смысла просить изображение снова
                    break;
                }
                catch(TimeoutException e)
                {
                    // Время ожидания превышенно надо попробовать снова
                    log.Info($"getCameraImg.GetImg() timeout exception. VideoReg({camera.VideoRegIp}) is not available in this moment or ");
                }
                catch (OperationCanceledException e)
                {
                    break;
                }
                catch (Exception e)
                {
                    // Неизвестная ошибка надо попробывать снова
                    log.Info(e.Message);
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
