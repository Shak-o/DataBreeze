namespace DataBreeze.Application.Interfaces;

public interface IServerStoreService
{
    void AddServer(string address);
    string GetNextServer();
    void UpdateServer(string address, int id);
    string GetServer(int id);
}