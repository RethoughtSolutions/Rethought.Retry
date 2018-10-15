using System;
using System.Threading;
using System.Threading.Tasks;
using Optional;

namespace Rethought.Retry
{
    public static class RetryService
    {
        private static readonly ITrier Trier = new Trier();

        public static Task<Option<T>> RetryAsync<T>(
            Func<CancellationToken, Task<Option<T>>> function,
            int retries,
            TimeSpan timeout,
            CancellationToken cancellationToken)
        {
            return Trier.RetryAsync(function, retries, timeout, cancellationToken);
        }

        public static Task<Option<T>> RetryAsync<T>(
            Func<CancellationToken, Task<Option<T>>> function,
            int retries,
            TimeSpan timeout)
        {
            return Trier.RetryAsync(function, retries, timeout);
        }
    }
}