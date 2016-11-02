using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InstaBot.Core
{
    //http://stackoverflow.com/questions/1563191/cleanest-way-to-write-retry-logic
    public static class Retry
    {
        public static async Task Do(Action action, CancellationToken token, RetryPolicy retryPolicy)
        {
            await Do<object>(() =>
            {
                action();
                return null;
            }, token, retryPolicy);
        }

        public static async Task<T> Do<T>(Func<T> action, CancellationToken token)
        {
            return await Do(action, token, new RetryPolicy());
        }

        public static async Task<T> Do<T>(Func<T> action, CancellationToken token, RetryPolicy retryPolicy)
        {
            token.ThrowIfCancellationRequested();

            if (action == null)
                throw new ArgumentNullException(nameof(action), "action must be not null");
            if (retryPolicy == null)
                throw new ArgumentNullException(nameof(retryPolicy), "retryPolicy must be not null");

            var exceptions = new List<Exception>();
            var timeoutToken = new CancellationTokenSource(retryPolicy.ActionExecutionTimeout);

            for (var retry = 0; retry < retryPolicy.RetryCount; retry++)
            {
                try
                {
                    if (retry > 0)
                        await Task.Delay(retryPolicy.RetryInterval, token);

                    var task = new Task<T>(action, timeoutToken.Token);
                    var result = await task;
                    return result;
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