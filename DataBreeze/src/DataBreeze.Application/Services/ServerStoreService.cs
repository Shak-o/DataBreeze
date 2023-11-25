using System.Collections.Concurrent;
using DataBreeze.Application.Interfaces;

namespace DataBreeze.Application.Services;

public class ServerStoreService : IServerStoreService
{
    private readonly ConcurrentDictionary<string, List<int>> _serverStore = new();
    private int _lastServerIndex = -1;
    
    public void AddServer(string address)
    {
        _serverStore.TryAdd(address, new List<int>());
    }

    public string GetNextServer()
    {
        // If reached end reset.
        if (_lastServerIndex == _serverStore.Keys.Count - 1)
            _lastServerIndex = -1;
        
        var server = _serverStore.Keys.ElementAt(++_lastServerIndex);

        return server;
    }

    public void UpdateServer(string address, int id)
    {
        _serverStore[address].Add(id);
    }

    public void RemoveId(string address, int id)
    {
        _serverStore[address].Remove(id);
    }

    public string GetServer(int id)
    {
        return _serverStore.First(x => x.Value.Contains(id)).Key;
    }
}