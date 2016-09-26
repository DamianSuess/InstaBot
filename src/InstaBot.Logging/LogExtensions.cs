using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaBot.Logging
{
    public static class LogExtensions
    {
        public static void Critical(this ILogger logger, Exception ex)
        {
            logger.Log(LogLevel.Fatal, ex.ToString());
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
            logger.Log(LogLevel.Error, ex.ToString());
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

