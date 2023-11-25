using Grpc.Net.Client;

namespace DataBreeze.Persistence.Interfaces;

public interface IGrpcChannelFactory
{
    GrpcChannel GetChannel(string serverAddress);
}