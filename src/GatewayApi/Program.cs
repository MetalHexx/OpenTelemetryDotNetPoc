using GatewayApi.Features.Weather;
using GatewayApi.Telemetry;
using GatewayApi.Telemetry.Filters;
using GatewayApi.Telemetry.Middleware;
using GatewayApi.Telemetry.Tracing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => 
{
    options.Filters.Add(typeof(MetricsFilter));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddTelemetry();

builder.Services.AddSingleton<IActivityService, ActivityService>();
builder.Services.AddSingleton<IWeatherService, WeatherService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseTraceIdMiddleware();
//app.UseHttpLogging();  If you want to see HTTP logs, uncomment this.  Good for determining if your /metrics endpoint is being scraped.

app.MapControllers();

app.Run();
