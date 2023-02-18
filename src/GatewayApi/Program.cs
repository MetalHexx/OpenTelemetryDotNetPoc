using GatewayApi.Telemetry;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => 
{
    options.Filters.Add(typeof(TelemetryFilter));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTelemetry();

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
app.UseRandomWaitMiddleware();

app.Run();
