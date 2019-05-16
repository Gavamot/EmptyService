using System.Threading.Tasks;
using VideoReg.Core;

namespace VideoReg.Api
{
    public class TestCameraRep : ICamerasRep
    {
        public async Task<CameraSettings[]> GetCameraSettingsAsync()
        {
            return new CameraSettings[] {
                new CameraSettings { Number = 1, SnapshotUrl = "http://192.168.88.242/webcapture.jpg?command=snap&channel=0"},
                new CameraSettings { Number = 2, SnapshotUrl = "http://192.168.20.31/webcapture.jpg?command=snap&channel=0"},
            };
        }
    }
}
