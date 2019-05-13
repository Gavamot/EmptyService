using System.Collections.Generic;

namespace OnlineCamera.Core
{
    public interface IOnlineCameraService
    {
        void AddVideoReg(VideoRegReqvestSettings settings);
        void AddVideoRegs(IEnumerable<VideoRegReqvestSettings> settings);
    }
}