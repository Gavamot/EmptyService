using OnlineCamera.Core.Value.Api;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    public interface IStatistRegistrator
    {
        Task RegAsync(string ip, VideoRegResponce responce);
    }
}