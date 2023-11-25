using DataBreeze.Application.Interfaces;
using DataBreeze.Grpc;
using DataBreezeBalancer.Grpc;
using Grpc.Core;
using Grpc.Net.Client;

namespace DataBreeze.LoadBalancer.Services;

public class BreezeCacheServerService : DataBreezeRpcForBalancer.DataBreezeRpcForBalancerBase
{
    private readonly IServerStoreService _serverStore;

    public BreezeCacheServerService(IServerStoreService serverStore)
    {
        _serverStore = serverStore;
    }

    public override async Task<GetResponseBalancer> Get(GetRequestBalancer request, ServerCallContext context)
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:7177");
        var client = new DataBreezeRpc.DataBreezeRpcClient(channel);
        var response = await client.GetAsync(new GetRequest() { Id = request.Id });
        return new GetResponseBalancer() { CachedData = response.CachedData };
    }

    public override async Task<SaveResponseBalancer> Save(SaveRequestBalancer request, ServerCallContext context)
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:7177");
        var client = new DataBreezeRpc.DataBreezeRpcClient(channel);
        var response = await client.SaveAsync(new SaveRequest() { Id = request.Id, Data = request.Data });
        return new SaveResponseBalancer() { IsSuccess = response.IsSuccess };
    }

    public override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
    {
        _serverStore.AddServer(request.Address);
        return Task.FromResult(new RegisterResponse() {Result = true});
    }
}