using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InstaBot.Core
{
    //http://stackoverflow.com/questions/1563191/cleanest-way-to-write-retry-logic
    public static class Retry
    {
        public static async Task Do(Action action, TimeSpan retryInterval, int retryCount = 3)
        {
            await Do<object>(() =>
            {
                action();
                return null;
            }, retryInterval, retryCount);
        }

        public static async Task<T> Do<T>(Func<T> action, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (var retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if (retry > 0)
                        await Task.Delay(retryInterval);
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }
    }
}