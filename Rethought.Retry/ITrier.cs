using System;
using System.Threading;
using System.Threading.Tasks;
using Rethought.Optional;

namespace Rethought.Retry
{
    public interface ITrier
    {
        Task<Option<T>> RetryAsync<T>(
            Func<CancellationToken, Task<Option<T>>> function,
            int retries,
            TimeSpan timeout,
            CancellationToken cancellationToken);

        Task<Option<T>> RetryAsync<T>(Func<CancellationToken, Task<Option<T>>> function, int retries, TimeSpan timeout);
    }
}