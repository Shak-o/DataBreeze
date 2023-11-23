using DataBreeze.Application.Interfaces;
using Grpc.Core;

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
}