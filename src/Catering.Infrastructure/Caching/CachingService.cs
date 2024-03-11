using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Catering.Application;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Catering.Infrastructure.Caching;

internal class CachingService : ICachingService
{
    private readonly IDistributedCache _cache;
    private readonly CachingSettings _settings;

    public CachingService(IDistributedCache cache, IOptions<CachingSettings> settings)
    {
        _cache = cache;
        _settings = settings.Value;
    }

    public Task SetAsync<T>(string key, T value) 
        => SetAsync(key, value, GetEntryOptions());

    public Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, GetJsonSerializerOptions()));
        return _cache.SetAsync(key, bytes, options);
    }

    public bool TryGetValue<T>(string key, out T value)
    {
        var byteValue = _cache.Get(key);

        value = default;
        if (byteValue == null) 
            return false;

        value = JsonSerializer.Deserialize<T>(byteValue, GetJsonSerializerOptions());
        return true;
    }

    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions()
        {
            PropertyNamingPolicy = null,
            WriteIndented = true,
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    private DistributedCacheEntryOptions GetEntryOptions() => new()
    { 
        SlidingExpiration = TimeSpan.FromSeconds(_settings.ExpirationInSeconds) 
    };
}
