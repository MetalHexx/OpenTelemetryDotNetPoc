using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace GatewayApi.Telemetry
{
    public static class FilterContextExtensions
    {
        public static ActionFilterResponseInfo GetResponseInfo(this FilterContext context)
        {
            return new ActionFilterResponseInfo
            {
                Route = context.ActionDescriptor.AttributeRouteInfo?.Template ?? "unknown",
                ClassName = $"{context.RouteData.Values["controller"]}Controller",
                MethodName = context.RouteData.Values["action"]?.ToString() ?? "unknown",
                StatusCode = context.HttpContext.Response.StatusCode
            };
        }

        public static ActionFilterResponseInfo GetResponseInfo(this ExceptionContext context)
        {
            var info = GetResponseInfo(context as FilterContext);
            info.StatusCode = (int)HttpStatusCode.InternalServerError;
            return info;
        }
    }
}
