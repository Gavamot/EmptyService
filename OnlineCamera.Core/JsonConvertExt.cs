using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    static class JsonConvertExt
    {
        public static string ToJson(this object self)
        {
            return JsonConvert.SerializeObject(self);
        }

    }
}
