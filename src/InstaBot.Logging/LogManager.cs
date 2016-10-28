using System;
using System.Diagnostics;
using Common.Logging.Configuration;
using Common.Logging.NLog;
using Common.Logging.Simple;

namespace InstaBot.Logging
{
    public class LogManager
    {
        private const string ConfigFileKey = "configFile";
        private const string ConfigTypeKey = "configType";

        public static ILogger GetLogger(string name)
        {
            return new CommonLoggerWrapper(Common.Logging.LogManager.GetLogger(name));

        }

        public static ILogger GetLogger(Type type)
        {
            return new CommonLoggerWrapper(Common.Logging.LogManager.GetLogger(type));
        }

        public static void UseNLog()
        {
            UseNLog(null);
        }

        public static void UseNLog(string configFilePath)
        {
            var properties = new NameValueCollection();
            if (!string.IsNullOrEmpty(configFilePath))
            {
                properties.Add(ConfigTypeKey, "file");
                properties.Add(ConfigFileKey, configFilePath);
            }
            else
            {
                properties.Add(ConfigTypeKey, "inline");
            }
            
            Common.Logging.LogManager.Adapter = new NLogLoggerFactoryAdapter(properties);
        }
        public static void UseConsole()
        {
            Common.Logging.LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(Common.Logging.LogLevel.Trace, true, true, true, null, true);
        }

        public static object GetCurrentClassLogger()
        {
            var frame = new StackFrame(1, false);
            return new CommonLoggerWrapper(Common.Logging.LogManager.GetLogger(frame.GetMethod().DeclaringType));
        }
    }
}