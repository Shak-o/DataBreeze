using System.Collections.Concurrent;
using DataBreeze.Application.Interfaces;

namespace DataBreeze.Application.Services;

public class DataBreezeCacheService : IBreezeCacheService
{
    private readonly ConcurrentDictionary<int, string> _cache = new();

    public bool Add(int id, string data)
    {
        return _cache.TryAdd(id, data);
    }

    public string? Get(int id)
    {
        return _cache[id];
    }
}