namespace VideoReg.Core
{
    public interface ICameraCache
    {
        void SetCamera(int cameraNumber, byte[] img);
        /// <exception cref="KeyNotFoundException">Thrown when key not found in collection</exception>
        CameraResponce GetCamera(int cameraNumber);
    }
}
