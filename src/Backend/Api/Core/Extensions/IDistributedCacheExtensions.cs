using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Api.Core.Extensions;

public static class IDistributedCacheExtensions
{
    public static async Task<TValue?> GetAsync<TValue>(this IDistributedCache cache, string key)
    {
        var str = await cache.GetStringAsync(key);
        return str is null ? default : JsonSerializer.Deserialize<TValue>(str);
    }

    public static Task SetAsync<TValue>(this IDistributedCache cache, string key, TValue value, DistributedCacheEntryOptions? options = null)
    {
        var str = JsonSerializer.Serialize(value);
        return options is null ?
            cache.SetStringAsync(key, str) :
            cache.SetStringAsync(key, str, options);
    }
}
