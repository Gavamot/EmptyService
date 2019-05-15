using System;
using System.Collections.Generic;
using System.Text;

namespace VideoReg.Core
{
    public interface IImgRep
    {
        CameraResponce GetImg(int number);
    }
}
