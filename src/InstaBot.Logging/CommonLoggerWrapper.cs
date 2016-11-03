using System;
using Common.Logging;

namespace InstaBot.Logging
{
    public class CommonLoggerWrapper : ILogger
    {
        private readonly ILog _innerLogger;

        internal CommonLoggerWrapper(ILog logger)
        {
            _innerLogger = logger;
        }

        #region ILogger Members

        public void Log(LogLevel level, string msg, Exception exception = null, params object[] args)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    _innerLogger.Trace(m => m(msg, args), exception);
                    break;
                case LogLevel.Debug:
                    _innerLogger.Debug(m => m(msg, args), exception);
                    break;
                case LogLevel.Info:
                    _innerLogger.Info(m => m(msg, args), exception);
                    break;
                case LogLevel.Warning:
                    _innerLogger.Warn(m => m(msg, args), exception);
                    break;
                case LogLevel.Error:
                    _innerLogger.Error(m => m(msg, args), exception);
                    break;
                case LogLevel.Fatal:
                    _innerLogger.Fatal(m => m(msg, args), exception);
                    break;
            }
        }


        #endregion
    }
}