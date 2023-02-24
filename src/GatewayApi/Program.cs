using GatewayApi.Features.Weather;
using GatewayApi.Telemetry.Extensions;
using GatewayApi.Telemetry.Metrics;
using GatewayApi.Telemetry.Tracing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => 
{
    options.Filters.Add(typeof(EndpointMetricsFilter));
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

//app.UseOpenTelemetryPrometheusScrapingEndpoint();  You can use this to expose a prometheus scrape endpoint if desired.  This is an alternative to using an Otel exporter.
app.UseTraceIdMiddleware();
//app.UseHttpLogging();  //If you want to see traditional HTTP logs, uncomment this.  Good for determining if your /metrics endpoint is being scraped.

app.MapControllers();

app.Run();
