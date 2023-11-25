using System.Collections.Concurrent;
using DataBreeze.Persistence.Interfaces;
using Grpc.Net.Client;

namespace DataBreeze.Persistence.Implementation;

public class GrpcChannelFactory : IGrpcChannelFactory
{
    private readonly ConcurrentDictionary<string, GrpcChannel> _channels = new();

    public GrpcChannel GetChannel(string serverAddress)
    {
        return _channels.GetOrAdd(serverAddress, GrpcChannel.ForAddress);
    }
}