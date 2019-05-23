using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoReg.Core
{
    public interface IAppLogger
    {
        /// <summary>
        /// tracing information and debugging minutiae; generally only switched on in unusual situations
        /// </summary>
        void Verbose(string message);

        /// <summary>
        /// internal control flow and diagnostic state dumps to facilitate pinpointing of recognised problems
        /// </summary>
        void Debug(string message);

        /// <summary>
        /// events of interest or that have relevance to outside observers; the default enabled minimum logging level
        /// </summary>
        void Information(string message);

        /// <summary>
        /// events of interest or that have relevance to outside observers; the default enabled minimum logging level
        /// </summary>
        void Warning(string message);

        /// <summary>
        ///  indicating a failure within the application or connected system
        /// </summary>
        void Error(string message, Exception e = null);

        /// <summary>
        /// critical errors causing complete failure of the applicationem
        /// </summary>
        void Fatal(string message);

    }
}
