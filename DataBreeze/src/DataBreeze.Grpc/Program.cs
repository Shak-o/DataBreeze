using DataBreeze.Application.Interfaces;
using DataBreeze.Application.Services;
using DataBreeze.Domain.Options;
using DataBreeze.Grpc.BackgroundTasks;
using DataBreeze.Grpc.Middlewares;
using DataBreeze.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<IBreezeCacheService, DataBreezeCacheService>();
builder.Services.AddSingleton<IRegistrationMonitor, RegistrationMonitor>();

builder.Services.Configure<BalancerOptions>(builder.Configuration.GetSection("Balancer"));
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<InitiatorBackgroundTask>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseMiddleware<ServerAddressesMiddleware>();
app.MapGrpcService<BreezeCacheService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();