using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace GatewayApi.Telemetry
{
    public static class FilterContextExtensions
    {
        public static FilterContextInfo GetFilterContextInfo(this FilterContext context)
        {
            return new FilterContextInfo
            {
                Route = context.ActionDescriptor.AttributeRouteInfo?.Template ?? "unknown",
                ClassName = $"{context.RouteData.Values["controller"]}Controller",
                MethodName = context.RouteData.Values["action"]?.ToString() ?? "unknown",
                StatusCode = context.HttpContext.Response.StatusCode
            };
        }
    }
}
