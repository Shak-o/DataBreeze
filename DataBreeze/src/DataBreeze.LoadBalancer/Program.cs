using DataBreeze.Application.Interfaces;
using DataBreeze.Application.Services;
using DataBreeze.Grpc;
using DataBreeze.LoadBalancer.Services;
using DataBreeze.Persistence.Implementation;
using DataBreeze.Persistence.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<IServerStoreService, ServerStoreService>();
builder.Services.AddSingleton<IGrpcChannelFactory, GrpcChannelFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<BreezeCacheServerService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();