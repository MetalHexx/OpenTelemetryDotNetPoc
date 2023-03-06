using OpenTelemetry.Trace;
using Hellang.Middleware.ProblemDetails;

namespace GatewayApi.Telemetry.Extensions
{
    public static class ProblemDetailStartupExtension
    {
        /// <summary>
        /// Configures the problem details middleware
        /// </summary>        
        public static void ConfigureProblemDetails(this IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) => ctx.RequestServices
                    .GetRequiredService<IHostEnvironment>()
                    .IsDevelopment();

                options.ShouldLogUnhandledException = (ctx, ex, problemDetails) => false;

                options.GetTraceId = context =>
                {
                    var traceId = Tracer.CurrentSpan.Context.TraceId.ToString();
                    context.Response.Headers.Add("X-Trace-Id", traceId);
                    return traceId;
                };
            });
        }
    }
}
