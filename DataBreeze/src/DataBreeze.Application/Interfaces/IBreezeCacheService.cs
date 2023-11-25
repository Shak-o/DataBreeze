namespace DataBreeze.Application.Interfaces;

public interface IBreezeCacheService
{
    bool Add(int id, string data);
    string? Get(int id);
    bool Remove(int id);
    bool Update(int id, string data);
}