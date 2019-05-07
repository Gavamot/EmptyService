using OnlineCamera.Core;
using System;

namespace OnlineCamera.Core
{
    public class DateService : IDateService
    {
        public DateTime GetCurrentDateTime() => DateTime.Now;
    }
}
