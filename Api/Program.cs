using System.Text.Json;
using Api.BackGroundService;
using Api.DataBase.Context;
using Api.Entities;
using Elastic.Apm.Api;
using Elastic.Apm.NetCoreAll;
using Elastic.Apm.StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SensorDbContext>(q => q.UseSqlite(builder.Configuration.GetConnectionString("SqlLite")!));
builder.Services.AddHostedService<SendSMSScheduler>();
var multiplexer =
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!);
multiplexer.UseElasticApm();
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAllElasticApm(builder.Configuration);
app.UseHttpsRedirection();
app.MapGet("/SetSensorData", async (SensorDbContext dbContext, IConnectionMultiplexer redis) =>
    {
        HttpClient client = new HttpClient();
        var db = redis.GetDatabase();
        
        string url = "https://jsonplaceholder.typicode.com/posts/1";
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        
     
        var sensorData = SensorDataGenerator.GenerateRandomSensorData();
        await db.StringSetAsync("sensor_data", JsonSerializer.Serialize(sensorData));
        await dbContext.SensorData.AddAsync(sensorData);
        await dbContext.SaveChangesAsync();
        
        var transaction = Elastic.Apm.Agent.Tracer.CurrentTransaction;
        await transaction.CaptureSpan("محاسبه وضعیت IOT", "محاسبه وضعیت IOT", async (span) =>
        {
            await Task.Delay(1000);
            span.Outcome = Outcome.Success;
        });
        
        return await dbContext.SensorData.ToListAsync();
    })
    .WithName("SetPersons")
    .WithOpenApi();

app.Run();