using OnlineCamera.Core;
using OnlineCamera.Core.Config.VideoRegUpdatorConfig;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    public class CameraNotFoundException : Exception
    {
        public CameraNotFoundException(string ip, int cameraNumber)
            : base($"VideoReg[{ip}] camera[{cameraNumber}] have no the image.")
        {

        }
    }

    public interface IGetCameraImg
    {
        Task<byte[]> GetImg(Size size, int delayMs, CancellationTokenSource source);
    }

    public interface IGetVideoRegCameras
    {
        Task<int[]> GetCameras(CancellationTokenSource source);
    }

    public interface VideoRegApi : IGetCameraImg, IGetVideoRegCameras { }

    public class CameraUpdatetor : AbstractUpdator
    {
        readonly Camera camera;
        readonly Size size;

        IGetCameraImg getCameraImg;
        VideoRegUpdatorConfig config;

        public CameraUpdatetor(ILogger log, IGetCameraImg getCameraImg, VideoRegUpdatorConfig config) : base(log)
        {
            this.getCameraImg = getCameraImg;
            this.config = config;
        }

        Task task { get; set; }

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
                    var img = await getCameraImg.GetImg(size, config.DelayMs, source);

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
