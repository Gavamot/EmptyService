using System.Collections.Concurrent;
using System.Linq;
using System.Collections.Generic;

namespace OnlineCamera.Core
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
