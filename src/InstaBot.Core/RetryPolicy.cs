using System;

namespace InstaBot.Core
{
    public class RetryPolicy
    {
        public RetryPolicy()
        {
            RetryCount = 3;
            RetryInterval = new TimeSpan(0, 1, 0);
            ActionExecutionTimeout = new TimeSpan(0, 1, 0);
        }

        public RetryPolicy(short retryCount, TimeSpan retryInterval, TimeSpan actionExecutionTimeout)
        {
            if (retryCount < 0)
                throw new ArgumentOutOfRangeException(nameof(retryCount), "Value must be greater than or equal to zero.");

            RetryCount = retryCount;
            RetryInterval = retryInterval;
            ActionExecutionTimeout = actionExecutionTimeout;
        }

        public short RetryCount { get; private set; }
        public TimeSpan RetryInterval { get; private set; }
        public TimeSpan ActionExecutionTimeout { get; private set; }
    }
}