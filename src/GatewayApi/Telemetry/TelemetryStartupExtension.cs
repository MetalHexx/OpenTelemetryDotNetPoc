using Microsoft.AspNetCore.HttpLogging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;
using GatewayApi.Telemetry.Metrics;
using GatewayApi.Telemetry.Logging;
using GatewayApi.Telemetry.Tracing;
using static GatewayApi.Telemetry.TelemetryConstants;

namespace GatewayApi.Telemetry
{
    public static class TelemetryStartupExtension
    {
        public static void AddTelemetry(this WebApplicationBuilder builder)
        {
            var resource = ResourceBuilder
                .CreateDefault()
                .AddService(App_Source);
            
            builder.Logging.AddOpenTelemetry(options => 
            {
                options.SetResourceBuilder(resource);
                options.AddConsoleExporter();
                options.AddProcessor(new LogProcessor());
                options.IncludeFormattedMessage = true;
                options.ParseStateValues = true;
            });

            builder.Logging.AddFilter<OpenTelemetryLoggerProvider>("*", LogLevel.Warning);

            builder.Services.AddTelemetry(resource);
        }
        
        public static void AddTelemetry(this IServiceCollection services, ResourceBuilder? resource)
        {
            services.AddOpenTelemetryMetrics(builder =>
            {
                builder
                .AddMeter(App_Source)
                .SetResourceBuilder(resource)
                .AddAspNetCoreInstrumentation()
                .AddPrometheusExporter(); //This creates a prometheus scrape endpoint.  The collector will scrape this and produce it's own scrape endpoint.
                //.AddConsoleExporter();  //Uncomment this if you want to see metrics exported to the console for debugging.
            });

            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                    .SetResourceBuilder(resource)
                    .AddAspNetCoreInstrumentation()
                    .AddSource(App_Source)
                    .AddProcessor(new TraceProcessor())
                    .AddJaegerExporter(options =>  //This exports tracing to Jaeger.
                    {
                        options.AgentHost = "poc-jaeger";
                        options.AgentPort = 6831;
                        
                    });
                    //.AddConsoleExporter();  //Uncomment this if you want to see traces exported to the console for debugging.
            });

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.All;
            });

            services.AddScoped<IMetricsService, MetricsService>();
        }
    }
}
