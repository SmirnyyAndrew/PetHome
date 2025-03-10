using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;
using System.Text.Json;

namespace PetHome.Core.Redis;

public class CacheService(IDistributedCache _cache) : ICacheService
{
    private ConcurrentDictionary<string, bool> _cacheKeys = new();

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default) where T : class
    {
        var cachedValueString = await _cache.GetStringAsync(key, ct);

        T? cachedValue = cachedValueString is null
            ? null
            : JsonSerializer.Deserialize<T>(cachedValueString);

        return cachedValue;
    }


    public async Task<T?> GetOrSetAsync<T>(
        string key,
        Func<Task<T?>> factory,
        DistributedCacheEntryOptions options = null,
        CancellationToken ct = default) where T : class
    {
        options ??= new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(10) };

        var cachedValue = await GetAsync<T>(key, ct);
        if (cachedValue is not null)
            return cachedValue;

        var newCachedValue = await factory();
        if (newCachedValue is not null)
            await SetAsync(key, newCachedValue, options, ct);

        return newCachedValue;
    }


    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        await _cache.RemoveAsync(key, ct);
        _cacheKeys.TryRemove(key, out bool _);
    }


    public async Task SetAsync<T>(
        string key,
        T value,
        DistributedCacheEntryOptions options = null,
        CancellationToken ct = default) where T : class
    {
        options ??= new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(10) };

        string valueSerialized = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, valueSerialized, options, ct);

        _cacheKeys.TryAdd(key, true);
    }


    public async Task RemoveByPrefixAsync(string prefix, CancellationToken ct = default)
    {
        var removeTasks = _cacheKeys.Keys
            .Where(k => k.StartsWith(prefix))
            .Select(key => RemoveAsync(key, ct)); 
        await Task.WhenAll(removeTasks);
    }
}