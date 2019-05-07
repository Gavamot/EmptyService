using System;

namespace OnlineCamera.Service
{
    public class DateService : IDateService
    {
        public DateTime GetCurrentDateTime() => DateTime.Now;
    }
}
