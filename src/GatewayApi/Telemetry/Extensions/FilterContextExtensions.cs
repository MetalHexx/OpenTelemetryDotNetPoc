using GatewayApi.Telemetry.Metrics;
using Microsoft.AspNetCore.Mvc.Filters;
using static GatewayApi.Telemetry.Constants.TelemetryConstants;

namespace GatewayApi.Telemetry.Extensions
{
    public static class FilterContextExtensions
    {
        private static string? _env;
        public static EndpointMetricTags GetEndpointMetricTags(this FilterContext context)
        {
            return new EndpointMetricTags
            (
                StatusCode: context.HttpContext.Response.StatusCode,
                RouteTemplate:      context.ActionDescriptor.AttributeRouteInfo?.Template ?? Unknown,
                ClassName:  $"{context.RouteData.Values["controller"]}Controller",
                ClassMethodName: context.RouteData.Values["action"]?.ToString() ?? Unknown,
                ServiceName: Service_Name,
                Environment: _env!
            );
        }
    }
}
