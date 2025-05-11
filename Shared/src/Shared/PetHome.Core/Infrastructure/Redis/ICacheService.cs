using Microsoft.Extensions.Caching.Distributed;

namespace PetHome.Core.Infrastructure.Redis;
public interface ICacheService
{
    Task<T?> GetOrSetAsync<T>(
        string key,
        Func<Task<T?>> factory,
        DistributedCacheEntryOptions options = null,
        CancellationToken ct = default) where T : class;

    Task<T?> GetAsync<T>(
        string key,
        CancellationToken ct = default) where T : class;

    Task SetAsync<T>(
        string key,
        T value,
        DistributedCacheEntryOptions options = null,
        CancellationToken ct = default) where T : class;

    Task RemoveAsync(string key, CancellationToken ct = default);

    Task RemoveByPrefixAsync(string prefix, CancellationToken ct = default);
}
