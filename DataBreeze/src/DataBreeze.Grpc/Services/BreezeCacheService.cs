using DataBreeze.Application.Interfaces;
using DataBreezeBalancer.Grpc;
using Grpc.Core;
using Grpc.Net.Client;

namespace DataBreeze.Grpc.Services;

public class BreezeCacheService(IBreezeCacheService cache) : DataBreezeRpc.DataBreezeRpcBase
{
    public override Task<GetResponse> Get(GetRequest request, ServerCallContext context)
    {
        return Task.FromResult(new GetResponse() { CachedData = cache.Get(request.Id) });
    }

    public override Task<SaveResponse> Save(SaveRequest request, ServerCallContext context)
    {
        return Task.FromResult(new SaveResponse() { IsSuccess = cache.Add(request.Id, request.Data) });
    }

    public override Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
    {
        return Task.FromResult(new UpdateResponse() { Updated = cache.Update(request.Id, request.Data) });
    }

    public override Task<RemoveResponse> Remove(RemoveRequest request, ServerCallContext context)
    {
        return Task.FromResult(new RemoveResponse() { Removed = cache.Remove(request.Id) });
    }
}