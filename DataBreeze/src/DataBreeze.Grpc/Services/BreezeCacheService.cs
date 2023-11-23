using DataBreeze.Application.Interfaces;
using Grpc.Core;

namespace DataBreeze.Grpc.Services;

public class BreezeCacheService : DataBreezeRpc.DataBreezeRpcBase
{
    private readonly IBreezeCacheService _cache;

    public BreezeCacheService(IBreezeCacheService cache)
    {
        _cache = cache;
    }

    public override Task<GetResponse> Get(GetRequest request, ServerCallContext context)
    {
        return Task.FromResult(new GetResponse() { CachedData = _cache.Get(request.Id) });
    }

    public override Task<SaveResponse> Save(SaveRequest request, ServerCallContext context)
    {
        return Task.FromResult(new SaveResponse() { IsSuccess = _cache.Add(request.Id, request.Data) });
    }
}