using System;
using System.Diagnostics;
using System.Web;
using Elmah;

namespace VideoLab.Web.Services.Logging
{
    /// <summary>
    /// Log <see cref="Exception"/> objects.
    /// </summary>
    public sealed class LoggingService : ILoggingService
    {
        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Log(Exception exception)
        {
            // Log to Tracing.
            Trace.TraceError(exception.ToString());
            // Log to Elmah.
            ErrorSignal.FromCurrentContext().Raise(exception, HttpContext.Current);
        }
    }
}