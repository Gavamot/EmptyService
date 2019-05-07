using System.Threading;

namespace OnlineCamera.Service
{
    public interface IUpdator
    {
        void Start(CancellationTokenSource source);
        void Stop();
    }
}