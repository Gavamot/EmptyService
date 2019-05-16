using System;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VideoReg.Core
{
    /// <summary>
    /// Обновляет изображение с камер
    /// </summary>
    public class CameraUpdatetor : AbstractUpdator<CameraSettings>
    {

        const int CountOfTrys = 3;
        const int TimeoutMs = 1000;
        const int SleepIfError = 200;
        const int IntervalMs = 800;

        CameraSettings cameraSettings;

        readonly IImgRep imgRep;
        readonly IAppLogger log;
        readonly ICameraCache cameraCache;

        public CameraUpdatetor(
            IImgRep imgRep,
            ICameraCache cameraCache,
            IAppLogger log) : base(log)
        {
            this.imgRep = imgRep;
            this.cameraCache = cameraCache;
            this.log = log;
        }

        public override string Name => $"CameraUpdatetor {cameraSettings}";

        public event Action<CameraUpdatetor, CameraSettings> OnCompleteUpdate;

        protected override async Task UpdateAsync(CameraSettings parameters)
        {
            this.cameraSettings = parameters;
            int trys = 0; 
            while (trys < CountOfTrys)
            {
                try
                {
                    var img = await imgRep.GetImgAsync(parameters.SnapshotUrl, TimeoutMs);
                    this.cameraCache.SetCamera(cameraSettings.Number, img);
                    log.Debug($"{Name} {parameters} - got {img.Length}");
                    await Task.Delay(IntervalMs);
                }
                catch(TimeoutException e)
                {
                    log.Error($"{Name} TimeoutException ({e.Message})");
                    await Task.Delay(SleepIfError);
                }
                catch (Exception e)
                {
                    log.Error($"{Name} update error ({e.Message})", e);
                    await Task.Delay(SleepIfError);
                    // Неизвестная ошибка надо попробывать снова
                    trys++;
                }
            }
        }

        protected override void OnComplete()
        {
            OnCompleteUpdate(this, cameraSettings);
        }
    }
}
