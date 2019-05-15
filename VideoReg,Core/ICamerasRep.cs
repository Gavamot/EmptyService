using System;
using System.Collections.Generic;
using System.Text;

namespace VideoReg_Core
{
    public interface ICamerasRep
    {
        CameraSettings[] GetCameraSettings();
    }
}
