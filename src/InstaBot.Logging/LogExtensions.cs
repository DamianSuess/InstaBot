using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaBot.Logging
{
    public static class LogExtensions
    {
        private static void Log(this ILogger logger, LogLevel level, string msg, params object[] args)
        {
            logger.Log(level, msg, null, args);
        }
        private static void Log(this ILogger logger, LogLevel level, string msg, Exception exception)
        {
            logger.Log(level, msg, exception);
        }
        private static void Log(this ILogger logger, LogLevel level, Exception exception)
        {
            logger.Log(level, string.Empty, exception);
        }

        public static void Critical(this ILogger logger, Exception ex)
        {
            logger.Log(LogLevel.Fatal, ex);
        }
        public static void Critical(this ILogger logger, string message, Exception ex)
        {
            logger.Log(LogLevel.Fatal,message ,ex);
        }

        public static void Critical(this ILogger logger, string message, params object[] args)
        {
            logger.Log(LogLevel.Fatal, message, args);
        }

        public static void Debug(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Debug, message);
        }

        public static void Debug(this ILogger logger, string message, params object[] args)
        {
            logger.Log(LogLevel.Debug, message, args);
        }

        public static void Error(this ILogger logger, Exception ex)
        {
            logger.Log(LogLevel.Error, ex);
        }
        public static void Error(this ILogger logger, string message, Exception ex)
        {
            logger.Log(LogLevel.Error, message, ex);
        }

        public static void Error(this ILogger logger, string message, params object[] args)
        {
            logger.Log(LogLevel.Error, message, args);
        }

        public static void Info(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Info, message);
        }
        public static void Info(this ILogger logger, string message, params object[] args)
        {
            logger.Log(LogLevel.Info, message, args);
        }

        public static void Warning(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Warning, message);
        }

        public static void Warning(this ILogger logger, string message, params object[] args)
        {
            logger.Log(LogLevel.Warning, message, args);
        }

        public static void Warning(this ILogger logger, string message, Exception exception)
        {
            logger.Log(LogLevel.Warning, message, exception);
        }

        public static void Trace(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Trace, message);
        }

        public static void Trace(this ILogger logger, string message, params object[] args)
        {
            logger.Log(LogLevel.Trace, message, args);
        }
    }
}

