using System.Threading;

namespace OnlineCamera.Core
{
    public interface IUpdator
    {
        void Start(CancellationTokenSource source);
        void Stop();
    }
}