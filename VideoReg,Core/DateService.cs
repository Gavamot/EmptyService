using System;

namespace VideoReg.Core
{
    public class DateService : IDateService
    {
        public DateTime GetCurrentDateTime() => DateTime.Now;
    }
}
