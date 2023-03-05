using GatewayApi.Telemetry.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using static GatewayApi.Telemetry.Constants.TelemetryConstants;

namespace GatewayApi.Telemetry.Metrics
{
    public class LoggingFilter : IActionFilter, IResultFilter, IExceptionFilter
    {
        private readonly IDiagnosticContext _loggingContext;

        public LoggingFilter(IDiagnosticContext loggingContext)
        {
            _loggingContext = loggingContext;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        /// <summary>
        /// Recording log context for each exception result.  
        /// </summary>
        public void OnException(ExceptionContext context)
        {
            var tags = context.GetEndpointMetricTags() with
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            _loggingContext.Set(Route_Template_Tag, tags.RouteTemplate);
            _loggingContext.Set(Class_Tag, tags.ClassName);
            _loggingContext.Set(Class_Method_Tag, tags.ClassMethodName);
            _loggingContext.Set(Http_Status_Code_Tag, tags.StatusCode.ToString());
        }

        public void OnResultExecuting(ResultExecutingContext context) { }

        /// <summary>
        /// Recording log context on each non-exception result
        /// </summary>
        public void OnResultExecuted(ResultExecutedContext context)
        {
            var tags = context.GetEndpointMetricTags();
            _loggingContext.Set(Route_Template_Tag, tags.RouteTemplate);
            _loggingContext.Set(Class_Tag, tags.ClassName);
            _loggingContext.Set(Http_Status_Code_Tag, tags.StatusCode.ToString());
            _loggingContext.Set(Class_Method_Tag, tags.ClassMethodName);            
        }
    }
}
