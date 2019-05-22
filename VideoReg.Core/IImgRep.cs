using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VideoReg.Core
{
    public interface IImgRep
    {
        Task<byte[]> GetImgAsync(string url, int timeoutMs);
    }
}
