using Microsoft.Extensions.Caching.Distributed;

namespace Catering.Application;

public interface ICachingService
{
    public Task SetAsync<T>(string key, T value);
    public Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options);
    public bool TryGetValue<T>(string key, out T value);
}
