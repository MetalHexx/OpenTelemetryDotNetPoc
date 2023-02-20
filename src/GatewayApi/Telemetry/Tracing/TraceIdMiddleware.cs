using OpenTelemetry.Trace;

namespace GatewayApi.Telemetry.Tracing
{
    public static class TraceIdMiddlewareExtensions
    {
        public static IApplicationBuilder UseTraceIdMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TraceIdMiddleware>();
        }
    }
    /// <summary>
    /// Adds a middleware to expose the trace id as a header on the response.
    /// </summary>
    public class TraceIdMiddleware
    {
        private readonly RequestDelegate _next;

        public TraceIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var currentSpan = Tracer.CurrentSpan;
            if (currentSpan != null)
            {
                context.Response.Headers.Add("X-Trace-Id", currentSpan.Context.TraceId.ToString());
            }

            await _next(context);
        }
    }
}
