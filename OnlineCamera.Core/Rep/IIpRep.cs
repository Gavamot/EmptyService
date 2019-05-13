using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    public interface IIpRep
    {
        VideoRegReqvestSettings[] GetAll();
    }
    
}
