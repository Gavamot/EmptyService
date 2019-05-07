using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCamera.Core
{
    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public override string ToString() => $"{Width}x{Height}";
        
    }
}
