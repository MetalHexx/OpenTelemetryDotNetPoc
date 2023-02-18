using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace GatewayApi.Telemetry
{
    public static class TelemetryStartupExtension
    {
        public static void AddTelemetry(this IServiceCollection services)
        {
            var resource = ResourceBuilder
                .CreateDefault()
                .AddService(TelemetryConstants.AppSource);

            services.AddOpenTelemetryMetrics(builder =>
            {
                builder
                .AddMeter(TelemetryConstants.AppSource)
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
                    .AddJaegerExporter(options =>  //This exports tracing to Jaeger.
                    {
                        options.AgentHost = "poc-jaeger";
                        options.AgentPort = 6831;
                    });
                    //.AddConsoleExporter();  //Uncomment this if you want to see traces exported to the console for debugging.
            });
        }
    }
}
