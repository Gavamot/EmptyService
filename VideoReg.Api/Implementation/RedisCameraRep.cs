using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoReg.Core;

namespace VideoReg.Api
{
    public class RedisCameraRep : ICamerasRep
    {
        RedisManagerPool pool;
        public RedisCameraRep()
        {
            pool = new RedisManagerPool("127.0.0.1:6379");
        }

        public CameraSettings[] GetCameraSettings()
        {
            using (var client = pool.GetClient())
            {
                var json = client.Get<string>("CamInfo");
                return JsonConvert.DeserializeObject<CameraSettings[]>(json);
            }
        }
    }
}
