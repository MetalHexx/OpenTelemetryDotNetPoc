using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using GatewayApi.Telemetry.Metrics;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using GatewayApi.Telemetry.Logging;
using OpenTelemetry.Logs;
using GatewayApi.Telemetry.Tracing;
using static GatewayApi.Telemetry.Constants.TelemetryConstants;

namespace GatewayApi.Telemetry.Extensions
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
                options.AddProcessor(new LogProcessor());
                options.IncludeFormattedMessage = true;
                options.ParseStateValues = true;
                //options.AddConsoleExporter();
            });

            builder.Logging.AddFilter<OpenTelemetryLoggerProvider>("*", LogLevel.Information);  //Increase the logging level to reduce noise

            builder.Services.AddTelemetry(resource);
        }

        public static void AddTelemetry(this IServiceCollection services, ResourceBuilder? resource)
        {
            services.AddOpenTelemetryMetrics(builder =>
            {
                builder
                .AddMeter(App_Source)
                .SetResourceBuilder(resource!)
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri("http://poc-collector:4319/v1/metrics");
                    options.Protocol = OtlpExportProtocol.HttpProtobuf;
                    options.ExportProcessorType = ExportProcessorType.Simple;
                    
                    //You can optionally batch exports for efficiency.
                    //options.ExportProcessorType = ExportProcessorType.Batch;
                    //options.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>()
                    //{
                    //    MaxQueueSize = 10,
                    //    ScheduledDelayMilliseconds = 1000,
                    //    ExporterTimeoutMilliseconds = 1000
                    //};
                })
                //.AddPrometheusExporter(); //Creates a prometheus exporter. The collector will scrape this and produce it's own scrape endpoint.  See: app.UseOpenTelemetryPrometheusScrapingEndpoint() in program.cs
                .AddConsoleExporter();  //Exports logs to console exporter.  Useful for debugging.
            });

            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                    .SetResourceBuilder(resource!)
                    .AddAspNetCoreInstrumentation()
                    .AddSource(App_Source)
                    .AddProcessor(new TraceProcessor())
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("http://poc-collector:4319/v1/traces");
                        options.Protocol = OtlpExportProtocol.HttpProtobuf;
                        options.ExportProcessorType = ExportProcessorType.Simple; //See: https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/sdk.md#built-in-span-processors
                    });
                    //.AddConsoleExporter();  //Uncomment this if you want to see traces exported to the console for debugging.
            });

            //Uncomment to see traditional http logging output
            //services.AddHttpLogging(options =>
            //{
            //    options.LoggingFields = HttpLoggingFields.All;
            //});

            services.AddScoped<IEndpointMetricsService, EndpointMetricsService>();
        }
    }
}
