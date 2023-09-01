using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using GatewayApi.Telemetry.Metrics;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using GatewayApi.Telemetry.Tracing;
using Serilog;
using static GatewayApi.Telemetry.Constants.TelemetryConstants;
using Serilog.Sinks.OpenTelemetry;

namespace GatewayApi.Telemetry.Extensions
{
    public static class TelemetryStartupExtension
    {   
        public static void AddTelemetry(this WebApplicationBuilder builder)
        {
            var resource = ResourceBuilder
                .CreateDefault()
                .AddService(App_Source);

            builder.AddLoggingTelemetry();
            builder.Services.AddMetricsTelemetry(resource);
            builder.Services.AddTracingTelemetry(resource);
        }
        public static void AddLoggingTelemetry(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, config) => config
                .ReadFrom.Configuration(context.Configuration)
                .MinimumLevel.Information()
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = "http://poc-collector:4319/v1/logs";
                    options.Protocol = OtlpProtocol.HttpProtobuf;
                    options.ResourceAttributes = new Dictionary<string, object>
                    {
                        ["service.name"] = App_Source
                    };
                    options.IncludedData = IncludedData.MessageTemplateTextAttribute
                        | IncludedData.TraceIdField
                        | IncludedData.SpanIdField;
                        

                }));
        }

        public static void AddMetricsTelemetry(this IServiceCollection services, ResourceBuilder? resource)
        {
            services.AddOpenTelemetryMetrics(builder =>
            {
                builder
                .AddMeter(App_Source)
                .SetResourceBuilder(resource!)
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter((exporterOptions, metricReaderOptions) =>
                {
                    exporterOptions.Endpoint = new Uri("http://poc-collector:4319/v1/metrics");
                    exporterOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                    exporterOptions.ExportProcessorType = ExportProcessorType.Simple;
                    metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1000;

                    //You can optionally batch exports for efficiency.
                    //options.ExportProcessorType = ExportProcessorType.Batch;
                    //options.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>()
                    //{
                    //    MaxQueueSize = 10,
                    //    ScheduledDelayMilliseconds = 1000,
                    //    ExporterTimeoutMilliseconds = 1000
                    //};
                });
                //.AddPrometheusExporter(); //Creates a prometheus exporter. The collector will scrape this and produce it's own scrape endpoint.  See: app.UseOpenTelemetryPrometheusScrapingEndpoint() in program.cs
                //.AddConsoleExporter();  //Exports logs to console exporter.  Useful for debugging.
            });

            services.AddScoped<IEndpointMetricsService, EndpointMetricsService>();
        }

        public static void AddTracingTelemetry(this IServiceCollection services, ResourceBuilder? resource)
        {
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
        }
    }
}
