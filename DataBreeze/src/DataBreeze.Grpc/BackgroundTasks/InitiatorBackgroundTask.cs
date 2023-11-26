using System.Net;
using System.Text;
using DataBreeze.Application.Interfaces;
using DataBreeze.Domain.Options;
using DataBreezeBalancer.Grpc;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace DataBreeze.Grpc.BackgroundTasks;

public class InitiatorBackgroundTask : BackgroundService
{
    private readonly IRegistrationMonitor _registrationMonitor;
    private readonly IOptions<BalancerOptions> _options;
    private readonly IOptions<ApplicationSettings> _applicationSettings;

    public InitiatorBackgroundTask(IRegistrationMonitor registrationMonitor,
        IOptions<BalancerOptions> options,  IOptions<ApplicationSettings> applicationSettings)
    {
        _registrationMonitor = registrationMonitor;
        _options = options;
        _applicationSettings = applicationSettings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var hostName = Dns.GetHostName();

        var hostEntry = await Dns.GetHostEntryAsync(hostName, stoppingToken);
        var ip = hostEntry.AddressList[0].ToString();
        var port = _applicationSettings.Value.Port;

        using var channel = GrpcChannel.ForAddress(_options.Value.Address);
        var client = new DataBreezeRpcForBalancer.DataBreezeRpcForBalancerClient(channel);
        
        var stringBuilder = new StringBuilder();

        // TODO: by architecture balancer and cache replicas will run on the same machine or in the same cluster so this must be ok.
        stringBuilder.Append(ip);
        stringBuilder.Append(port);

        await client.RegisterAsync(new RegisterRequest() { Address = stringBuilder.ToString() });
        _registrationMonitor.SetRegistered();
    }
}