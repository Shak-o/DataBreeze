using DataBreeze.Application.Interfaces;
using DataBreezeBalancer.Grpc;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace DataBreeze.Grpc.Middlewares;

public class ServerAddressesMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IRegistrationMonitor _registrationMonitor;

    public ServerAddressesMiddleware(RequestDelegate next, IRegistrationMonitor registrationMonitor)
    {
        _next = next;
        _registrationMonitor = registrationMonitor;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_registrationMonitor.IsRegistered())
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7011");
            var client = new DataBreezeRpcForBalancer.DataBreezeRpcForBalancerClient(channel);
            var ip = context.Connection.LocalIpAddress!.ToString();
            var port = context.Connection.LocalPort.ToString();
            client.Register(new RegisterRequest(){Address = $"{ip}:{port}"});
            _registrationMonitor.SetRegistered();
        }


        await _next(context);
    }
}