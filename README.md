# Rethought.Retry

A lightweight .Net Standard library that assists in retrying

[Install from Nuget](https://www.nuget.org/packages/Rethought.Retry/)

## Getting Started

Usage is quite simple. All you need is the static `RetryService` class.

```csharp
var result = await RetryService.RetryAsync<int>(async cancellationToken =>
    {
        // Some async operation that calculates a number
        Console.WriteLine("Calculation finished!");
        return 5;
    }, 
    3,
    TimeSpan.FromMinutes(5));

// RetryAsync returns an Option<T> to avoid null
// See below how to resolve the value
if (result.TryGetValue(out var value))
{
    Console.WriteLine($"The result is {value}.");
}
else
{
    Console.WriteLine("No result available.");
}
```

It is also possible to pass in a `CancellationToken` with an overload. 
This `CancellationToken` is then being passed into the Func where your main workload occurs. 

However, you don't have to use the `RetryService`. You can also create your own implementation of `ITrier`. 
RetryService uses the default implementation `Trier`.  

## Limitations

- The library is exclusively async. A workaround is to return `Task.CompletedTask` instead.
- The retry methods expect to return a result