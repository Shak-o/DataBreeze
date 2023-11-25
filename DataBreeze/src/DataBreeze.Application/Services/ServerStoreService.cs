using System.Collections.Concurrent;
using DataBreeze.Application.Interfaces;

namespace DataBreeze.Application.Services;

public class ServerStoreService : IServerStoreService
{
    private readonly ConcurrentDictionary<string, List<int>> _serverStore = new();
    
    public void AddServer(string address)
    {
        _serverStore.TryAdd(address, new List<int>());
    }
}