namespace InstaBot.Logging
{
    public class NullLogger : ILogger
    {
        #region ILogger Members

        public void Log(LogLevel level, string msg, params object[] args)
        {
        }

        #endregion
    }
}