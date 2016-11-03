using System;

namespace InstaBot.Logging
{
    public class NullLogger : ILogger
    {
        #region ILogger Members
        
        public void Log(LogLevel level, string msg, Exception exception = null, params object[] args)
        {
        }

        #endregion
    }
}