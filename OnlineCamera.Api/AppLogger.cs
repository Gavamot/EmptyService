using OnlineCamera.Core;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCamera.Api
{
    public class AppLogger : IAppLogger
    {
        ILogger log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\myapp.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

        public void Debug(string message)
        {
            log.Debug(message);
        }

        public void Error(string message, Exception e = null)
        {
            log.Error(message, e);
        }

        public void Fatal(string message)
        {
            log.Fatal(message);
        }

        public void Information(string message)
        {
            log.Information(message);
        }

        public void Verbose(string message)
        {
            log.Verbose(message);
        }

        public void Warning(string message)
        {
            log.Warning(message);
        }
    }
}
