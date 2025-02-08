using Microsoft.Extensions.Caching.Memory;
using Zust.BL.ExternalServices.Interfaces;

namespace Zust.BL.ExternalServices.Implements;

public class LocalCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    public LocalCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> Get<T>(string key)
    {
        T? value = default(T);
        await Task.Run(() =>
        {
            _cache.TryGetValue<T>(key, out value);
        });
        return value;
    }

    public async Task Set<T>(string key, T data, int seconds = 300)
    {
        await Task.Run(() =>
        {
            _cache.Set<T>(key, data, DateTime.Now.AddSeconds(seconds));
        });
    }

    public async Task<bool> IsExists<T>(string key)
    {
        T? data = await Task.Run(() => _cache.Get<T>(key));
        if (data == null)
            return false;

        return true;
    }

    public void Delete(string key)
        => _cache.Remove(key);
}
