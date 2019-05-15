using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCamera.Core.Service
{
    class ApiDataTransformer
    {
        const string DateFormat = "dd-MM-yyyyTHH24:mm:ss";
        public DateTime ToDate(string dt)
        {
            return DateTime.ParseExact(dt, DateFormat, CultureInfo.InvariantCulture);
        }

        public string ToString(DateTime dt)
        {
            return dt.ToString(DateFormat, CultureInfo.InvariantCulture);
        }
    }
}
