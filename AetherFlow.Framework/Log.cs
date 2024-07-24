 using AetherFlow.Framework.Interfaces;
using Microsoft.Xrm.Sdk;
using System;
 using AetherFlow.Framework.Configuration;

 namespace AetherFlow.Framework
{
    public class Log : ILog
    {
        private readonly ITracingService _tracingService;
        private readonly TraceConfiguration _config;
        private readonly string _format = "{0}___{1}___{2}";

        public Log(ITracingService tracingService, TraceConfiguration traceConfiguration)
        {
            _tracingService = tracingService;
            _config = traceConfiguration;
        }

        public void Info(object message)
            => Trace("INFO", message, _config.ShouldLogInfo);

        public void Info(object message, Exception exception)
            => Trace("INFO", message, exception, _config.ShouldLogInfo);

        public void InfoFormat(string format, params object[] args)
            => Trace("INFO", string.Format(format, args), _config.ShouldLogInfo);

        public void Debug(object message) 
            => Trace("DEBUG", message, _config.ShouldLogDebug);

        public void Debug(object message, Exception exception) 
            => Trace("DEBUG", message, exception, _config.ShouldLogDebug);

        public void DebugFormat(string format, params object[] args)
            => Trace("DEBUG", string.Format(format, args), _config.ShouldLogDebug);

        public void Error(object message)
            => Trace("ERROR", message, _config.ShouldLogError);

        public void Error(object message, Exception exception)
            => Trace("ERROR", message, exception, _config.ShouldLogError);

        public void ErrorFormat(string format, params object[] args)
            => Trace("ERROR", string.Format(format, args), _config.ShouldLogError);

        public void Fatal(object message)
            => Trace("FATAL", message, _config.ShouldLogFatal);

        public void Fatal(object message, Exception exception)
            => Trace("FATAL", message, exception, _config.ShouldLogFatal);

        public void FatalFormat(string format, params object[] args)
            => Trace("FATAL", string.Format(format, args), _config.ShouldLogFatal);

        private void Trace(string tracingLevel, object message, bool shouldLog)
        {
            if (!shouldLog) return;
            _tracingService.Trace(
                _format, 
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss,fff"), 
                tracingLevel, 
                message
            );
        }

        private void Trace(string tracingLevel, object message, Exception exception, bool shouldLog)
        {
            if (!shouldLog) return;
            Trace(tracingLevel, message, true);

            var outerException = GetExceptionMessage(exception);
            if (exception.InnerException == null)
            {
                _tracingService.Trace(outerException);
            }
            else
            {
                var innerException = GetExceptionMessage(exception.InnerException);
                _tracingService.Trace($"{outerException} ----> {innerException}");
                _tracingService.Trace("--- End of inner exception stack trace ---");
            }
        }

        private string GetExceptionMessage(Exception exception)
        {
            return $"{exception.GetType()}: {exception.Message} ----> {exception.StackTrace}";
        }
    }
}
