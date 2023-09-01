using GatewayApi.Telemetry.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using static GatewayApi.Telemetry.Constants.TelemetryConstants;

namespace GatewayApi.Telemetry.Metrics
{
    public class LoggingFilter : IActionFilter, IResultFilter, IExceptionFilter
    {
        private readonly IDiagnosticContext _loggingContext;
        private readonly IHostEnvironment _env;

        public LoggingFilter(IDiagnosticContext loggingContext, IHostEnvironment env)
        {
            _loggingContext = loggingContext;
            _env = env;
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
            AddTagsToLoggingContext(tags);
        }

        public void OnResultExecuting(ResultExecutingContext context) { }

        /// <summary>
        /// Recording log context on each non-exception result
        /// </summary>
        public void OnResultExecuted(ResultExecutedContext context)
        {
            var tags = context.GetEndpointMetricTags();
            AddTagsToLoggingContext(tags);            
        }

        public void AddTagsToLoggingContext(EndpointMetricTags tags)
        {
            _loggingContext.Set(Route_Template_Tag, tags.RouteTemplate);
            _loggingContext.Set(Class_Tag, tags.ClassName);
            _loggingContext.Set(Class_Method_Tag, tags.ClassMethodName);
            _loggingContext.Set(Http_Status_Code_Tag, tags.StatusCode.ToString());
            _loggingContext.Set(Service_Name_Tag, tags.ServiceName);
            _loggingContext.Set(Environment_Tag, _env.EnvironmentName);
        }
    }
}
