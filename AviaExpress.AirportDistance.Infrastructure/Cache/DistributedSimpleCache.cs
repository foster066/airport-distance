
using AviaExpress.AirportDistance.SharedKernel;
using Microsoft.Extensions.Caching.Hybrid;

namespace AviaExpress.AirportDistance.Infrastructure.Cache;

public class DistributedSimpleCache : ICache
{
    private readonly HybridCache _cache;

    public DistributedSimpleCache(HybridCache cache)
    {
        _cache = cache;
    }

    public async Task<TValue?> GetOrAdd<TKey, TValue>(TKey key, Func<TKey, CancellationToken, Task<TValue>> returnValueFunc, CancellationToken cancellationToken)
    {
        string? keyString = key as string ?? key?.ToString();
        if (string.IsNullOrWhiteSpace(keyString))
        {
            return default;
        }
        return await _cache.GetOrCreateAsync(keyString, 
            (key, service: this, returnValueFunc),
            static async (state, token) => await state.returnValueFunc(state.key, token),
            cancellationToken: cancellationToken);
    }
}
