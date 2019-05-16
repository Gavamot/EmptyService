using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoReg.Core;

namespace VideoReg.Api
{
    public class TestVideoRegInfo : IVideoRegInfoRep
    {
        public async Task<VideoRegResponce> GetInfo()
        {
            return new VideoRegResponce
            {
                BrigadeCode = 1,
                Cameras = new []{ 1, 2},
                CurrentDate = DateTime.Now,
                IveSerial = "FSW-2333",
                Version = "0.0.1 Beta",
                VideoRegSerial = "QQQ-QXE123"
            };
        }
    }
}
