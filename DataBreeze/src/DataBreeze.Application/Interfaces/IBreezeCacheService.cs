namespace DataBreeze.Application.Interfaces;

public interface IBreezeCacheService
{
    bool Add(int id, string data);
    string? Get(int id);
}