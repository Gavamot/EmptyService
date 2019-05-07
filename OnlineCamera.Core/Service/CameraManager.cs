using System.Collections.Concurrent;
using OnlineCamera.Value;
using System.Linq;
using System.Collections.Generic;

namespace OnlineCamera.Service
{
    

    public class CameraManager
    {
        readonly ConcurrentDictionary<Camera, CameraUpdatetor> tasks =
            new ConcurrentDictionary<Camera, CameraUpdatetor>();

        public void Start()
        {
         
        }


        public void Stop()
        {
           
        }

        public void Restart()
        {
            Stop();
            Start();
        }
    }
}
