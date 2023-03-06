using GatewayApi.Features.Weather;
using GatewayApi.Telemetry.Extensions;
using GatewayApi.Telemetry.Metrics;
using GatewayApi.Telemetry.Tracing;
using Hellang.Middleware.ProblemDetails;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web host");
    
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add(typeof(EndpointMetricsFilter));
        options.Filters.Add(typeof(LoggingFilter));
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.ConfigureProblemDetails();
    builder.AddTelemetry(); 

    builder.Services.AddSingleton<IActivityService, ActivityService>();
    builder.Services.AddSingleton<IWeatherService, WeatherService>();

    var app = builder.Build();

    app.UseProblemDetails();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging(config =>
    {
        config.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    });

    //app.UseOpenTelemetryPrometheusScrapingEndpoint();  You can use this to expose a prometheus scrape endpoint if desired.  This is an alternative to using an Otel exporter.
    //app.UseHttpLogging();  //If you want to see traditional HTTP logs, uncomment this.  Good for determining if your /metrics endpoint is being scraped.

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
return 0;
