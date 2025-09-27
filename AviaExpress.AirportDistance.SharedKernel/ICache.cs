
namespace AviaExpress.AirportDistance.SharedKernel;

/// <summary>
/// Abstract interface describing returning a value from cache if it exists.
/// If a value doesn't exist it also adds it.
/// </summary>
public interface ICache
{
    /// <summary>
    /// Returns a value from cache.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <param name="key">Key value.</param>
    /// <param name="returnValueFunc">Function initialization of a value if a key doesn't already exist.</param>
    /// <returns>A value from cache.</returns>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    Task<TValue?> GetOrAdd<TKey, TValue>(TKey key, Func<TKey, CancellationToken, Task<TValue>> returnValueFunc, CancellationToken cancellationToken);
}
