using OnlineCamera.Core.Value.Api;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    public interface IGetCameraImg
    {
        Task<CameraResponce> GetImgAsync(string ip, Size size, int timeoutMs, CancellationTokenSource source);
    }

    public interface IGetVideoRegInfo
    {
        Task<VideoRegResponce> GetVideoRegInfoAsync(string ip, int timeoutMs, CancellationTokenSource source);
    }

    public interface IVideoRegApi : IGetCameraImg, IGetVideoRegInfo { }
}