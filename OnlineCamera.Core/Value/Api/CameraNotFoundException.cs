using System;

namespace OnlineCamera.Core
{
    public class CameraNotFoundException : Exception
    {
        public CameraNotFoundException(string ip, int cameraNumber)
            : base($"VideoReg[{ip}] camera[{cameraNumber}] have no the image.")
        {

        }

        public CameraNotFoundException(Camera camera)
           : this(camera.VideoRegIp, camera.Number)
        {

        }
    }
}
