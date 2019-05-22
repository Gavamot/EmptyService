using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VideoReg.Core.Thrends
{
    public class IFileTrendsRep : ITrendsRep
    {
        const string FileName = "values.json";

        public async Task<byte[]> GetThrends()
        {
            return await File.ReadAllBytesAsync(FileName);
        }
    }
}
