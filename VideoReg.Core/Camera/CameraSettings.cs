using System;
using System.Collections.Generic;
using System.Text;

namespace VideoReg.Core
{
    public class CameraSettings
    {
        public int Number { get; set; }
        public string SnapshotUrl { get; set; }
        public override string ToString() => $"CameraNumber={Number} URL={SnapshotUrl}";
    }
}
