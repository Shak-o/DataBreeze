using DataBreeze.Application.Interfaces;
using DataBreeze.Application.Services;
using DataBreeze.Grpc.Middlewares;
using DataBreeze.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<IBreezeCacheService, DataBreezeCacheService>();
builder.Services.AddSingleton<IRegistrationMonitor, RegistrationMonitor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ServerAddressesMiddleware>();
app.MapGrpcService<BreezeCacheService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();