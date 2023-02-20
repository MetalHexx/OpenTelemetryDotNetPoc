using GatewayApi.Telemetry.Metrics;
using Microsoft.AspNetCore.Mvc.Filters;
using static GatewayApi.Telemetry.Constants.TelemetryConstants;

namespace GatewayApi.Telemetry.Extensions
{
    public static class FilterContextExtensions
    {
        public static EndpointMetricTags GetEndpointMetricTags(this FilterContext context)
        {
            return new EndpointMetricTags
            (
                StatusCode: context.HttpContext.Response.StatusCode,
                Route:      context.ActionDescriptor.AttributeRouteInfo?.Template ?? Unknown,
                ClassName:  $"{context.RouteData.Values["controller"]}Controller",
                MethodName: context.RouteData.Values["action"]?.ToString() ?? Unknown
            );
        }
    }
}
