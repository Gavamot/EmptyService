using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VideoReg.Core
{
    public interface ICamerasRep
    {
        Task<CameraSettings[]> GetCameraSettingsAsync();
    }
}
