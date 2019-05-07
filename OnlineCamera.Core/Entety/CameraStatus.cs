using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCamera.Entety
{
    public class CameraStatus
    {
        public int Id { get; set; }
        public string Ip { get;set; }
        public int Camera { get; set; }
        public int BrigadeCode { get; set; }
        public DateTime From { get; set; }
        public DateTime? To {get;set;}
        public bool IsActive { get; set; }
    }
}
