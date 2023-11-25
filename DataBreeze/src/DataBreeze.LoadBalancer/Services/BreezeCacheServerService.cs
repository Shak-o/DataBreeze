using DataBreeze.Application.Interfaces;
using DataBreeze.Grpc;
using DataBreeze.Persistence.Interfaces;
using DataBreezeBalancer.Grpc;
using Grpc.Core;
using Grpc.Net.Client;

namespace DataBreeze.LoadBalancer.Services;

public class BreezeCacheServerService : DataBreezeRpcForBalancer.DataBreezeRpcForBalancerBase
{
    private readonly IServerStoreService _serverStore;
    private readonly IGrpcChannelFactory _grpcChannelFactory;
    public BreezeCacheServerService(IServerStoreService serverStore, IGrpcChannelFactory grpcChannelFactory)
    {
        _serverStore = serverStore;
        _grpcChannelFactory = grpcChannelFactory;
    }

    public override async Task<GetResponseBalancer> Get(GetRequestBalancer request, ServerCallContext context)
    {
        var server = _serverStore.GetServer(request.Id);
        var channel = _grpcChannelFactory.GetChannel(server);
        
        var client = new DataBreezeRpc.DataBreezeRpcClient(channel);
        var response = await client.GetAsync(new GetRequest() { Id = request.Id });
        
        return new GetResponseBalancer() { CachedData = response.CachedData };
    }

    public override async Task<SaveResponseBalancer> Save(SaveRequestBalancer request, ServerCallContext context)
    {
        var server = _serverStore.GetNextServer();
        var channel = _grpcChannelFactory.GetChannel(server);
        
        var client = new DataBreezeRpc.DataBreezeRpcClient(channel);
        var response = await client.SaveAsync(new SaveRequest() { Id = request.Id, Data = request.Data });
        _serverStore.UpdateServer(server, request.Id);
        
        return new SaveResponseBalancer() { IsSuccess = response.IsSuccess };
    }

    public override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
    {
        _serverStore.AddServer(request.Address);
        return Task.FromResult(new RegisterResponse() {Result = true});
    }

    public override async Task<RemoveResponseBalancer> Remove(RemoveRequestBalancer request, ServerCallContext context)
    {
        var server = _serverStore.GetServer(request.Id);
        var channel = _grpcChannelFactory.GetChannel(server);
        
        var client = new DataBreezeRpc.DataBreezeRpcClient(channel);
        var response = await client.RemoveAsync(new RemoveRequest() { Id = request.Id });
        
        return new RemoveResponseBalancer() { Removed = response.Removed };
    }

    public override async Task<UpdateResponseBalancer> Update(UpdateRequestBalancer request, ServerCallContext context)
    {
        var server = _serverStore.GetServer(request.Id);
        var channel = _grpcChannelFactory.GetChannel(server);
        
        var client = new DataBreezeRpc.DataBreezeRpcClient(channel);
        var response = await client.UpdateAsync(new UpdateRequest() { Data = request.Data, Id = request.Id });

        return new UpdateResponseBalancer() { Updated = response.Updated };
    }
}