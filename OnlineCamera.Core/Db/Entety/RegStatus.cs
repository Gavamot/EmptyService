using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCamera.Core.Db.Entety
{
    public class RegStatus
    {
        public int Id { get; set; }
        public DateTime TimeStampt { get; set; }
        public string Ip { get; set; }
        public int BrigadeCode { get; set; }
        public string CamerasArray { get; set; }

        public int[] Cameras => CamerasArray
            .Split(',')
            .Select(int.Parse)
            .ToArray();
       
        public string VideoRegSerial { get; set; }

        public string IveSerial { get; set; }
    }
}
