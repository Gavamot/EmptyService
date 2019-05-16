using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace VideoReg.Core
{
    class AppLogger : IAppLogger
    {
        readonly ILogger log;
        public AppLogger(ILogger log)
        {
            this.log = log;
        }

        public void Debug(string message)
        {
            log.Log(LogLevel.Debug, message);
        }

        public void Error(string message, Exception e = null)
        {
            log.Log(LogLevel.Error, message, e);
        }

        public void Fatal(string message)
        {
            log.Log(LogLevel.Critical, message);
        }

        public void Information(string message)
        {
            log.Log(LogLevel.Information, message);
        }

        public void Verbose(string message)
        {
            log.Log(LogLevel.None, message);
        }

        public void Warning(string message)
        {
            log.Log(LogLevel.Warning, message);
        }
    }
}
