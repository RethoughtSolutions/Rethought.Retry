using System;
using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Rethought.Retry
{
    public class Trier : ITrier
    {
        public async Task<Option<T>> RetryAsync<T>(
            Func<CancellationToken, Task<Option<T>>> function,
            int retries,
            TimeSpan timeout,
            CancellationToken cancellationToken)
        {
            var internalCancellationTokenSource = new CancellationTokenSource();
            var cancellationTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken,
                    internalCancellationTokenSource.Token);

            var task = CreateRetryTask(cancellationTokenSource.Token, function, retries);

            var whenAnyTask = await Task.WhenAny(task, Task.Delay(timeout, cancellationTokenSource.Token))
                .ConfigureAwait(false);
            if (whenAnyTask == task)
            {
                var result = task.Result;
                cancellationTokenSource.Cancel();

                return result;
            }

            cancellationTokenSource.Cancel();
            return Option.None<T>();
        }

        public Task<Option<T>> RetryAsync<T>(
            Func<CancellationToken, Task<Option<T>>> function,
            int retries,
            TimeSpan timeout)
        {
            return RetryAsync(function, retries, timeout, CancellationToken.None);
        }

        private static async Task<Option<T>> CreateRetryTask<T>(
            CancellationToken cancellationToken,
            Func<CancellationToken, Task<Option<T>>> function,
            int retries)
        {
            do
            {
                var resultTask = await function(cancellationToken).ConfigureAwait(false);

                if (resultTask.HasValue)
                    return resultTask;

                retries--;
            } while (retries > 0 && !cancellationToken.IsCancellationRequested);

            return Option.None<T>();
        }
    }
}