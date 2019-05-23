using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoReg.Core;

namespace VideoReg.Api.Implementation
{
    public class VideoRegInfoRep : IVideoRegInfoRep
    {
        RedisManagerPool pool;
        public VideoRegInfoRep()
        {
            pool = new RedisManagerPool("127.0.0.1:6379");
        }

        public VideoRegResponce GetInfo()
        {
            using (var client = pool.GetClient())
            {
                var json = client.Get<string>("CamInfo");
                var cameras = JsonConvert.DeserializeObject<CameraSettings[]>(json);
                return new VideoRegResponce
                {
                    BrigadeCode = 0,
                    Cameras = cameras.Select(x => x.Number).ToArray(),
                    CurrentDate = DateTime.Now,
                    IveSerial = "XZ-14",
                    Version = "0.0.0.0.1 beta",
                    VideoRegSerial = "XZ-VIDEO"
                };
            }

            
        }
    }
}
