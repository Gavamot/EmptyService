using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VideoReg.Core.Thrends
{
    public interface ITrendsRep
    {
        Task<byte[]> GetThrends();
    }
}
