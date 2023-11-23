using DataBreezeBalancer.Grpc;
using Grpc.Net.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet("/GetCache", string (int id) =>
{
    using var channel = GrpcChannel.ForAddress("https://localhost:7011");
    var client = new DataBreezeRpcForBalancer.DataBreezeRpcForBalancerClient(channel);
    var response = client.Get(new GetRequestBalancer() { Id = id });
    return response.CachedData;
});

app.MapPost("/SaveCache", async Task<bool> (int id, string data) =>
{
    using var channel = GrpcChannel.ForAddress("https://localhost:7011");
    var client = new DataBreezeRpcForBalancer.DataBreezeRpcForBalancerClient(channel);
    var response = await client.SaveAsync(new SaveRequestBalancer() { Id = id, Data = data});
    return response.IsSuccess;
});
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}