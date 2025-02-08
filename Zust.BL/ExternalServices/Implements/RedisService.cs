using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Zust.BL.Exceptions.Common;
using Zust.BL.ExternalServices.Interfaces;
using Zust.Core.Entities;

namespace Zust.BL.ExternalServices.Implements;

public class RedisService : ICacheService
{
    private readonly IDistributedCache _db;
    public RedisService(IDistributedCache db)
    {
        _db = db;
    }

    public async Task<T?> Get<T>(string key)
    {
        string? jsonData = await _db.GetStringAsync(key);
        if (jsonData == null) throw new NotFoundException<User>("Item not found!");
        return JsonSerializer.Deserialize<T>(jsonData);
    }

    public async Task Set<T>(string key, T data, int seconds = 300)
    {
        string jsonData = JsonSerializer.Serialize(data);
        var opt = new DistributedCacheEntryOptions();
        opt.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(seconds);
        await _db.SetStringAsync(key, jsonData, opt);
    }

    public async Task<bool> IsExists<T>(string key)
    {
        string? jsonData = await _db.GetStringAsync(key);
        if (String.IsNullOrEmpty(jsonData) || String.IsNullOrWhiteSpace(jsonData))
            return false;

        return true;
    }

    public void Delete(string key)
        => _db.Remove(key);
}
