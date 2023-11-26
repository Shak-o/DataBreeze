using System.Text;
using DataBreeze.Application.Interfaces;
using DataBreeze.Domain.Options;
using DataBreezeBalancer.Grpc;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Options;

namespace DataBreeze.Grpc.Middlewares;

public class ServerAddressesMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IRegistrationMonitor _registrationMonitor;
    private readonly IOptions<BalancerOptions> _options;
    
    public ServerAddressesMiddleware(RequestDelegate next, IRegistrationMonitor registrationMonitor, IOptions<BalancerOptions> options)
    {
        _next = next;
        _registrationMonitor = registrationMonitor;
        _options = options;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_registrationMonitor.IsRegistered())
        {
            using var channel = GrpcChannel.ForAddress(_options.Value.Address);
            var client = new DataBreezeRpcForBalancer.DataBreezeRpcForBalancerClient(channel);
            
            var port = context.Connection.LocalPort.ToString();
            var stringBuilder = new StringBuilder();
            
            // TODO: by architecture balancer and cache replicas will run on the same machine or in the same cluster so this must be ok.
            stringBuilder.Append("http://localhost:");
            stringBuilder.Append(port);
            
            await client.RegisterAsync(new RegisterRequest(){Address = stringBuilder.ToString()});
            _registrationMonitor.SetRegistered();
        }


        await _next(context);
    }
}