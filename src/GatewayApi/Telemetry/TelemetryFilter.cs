using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Net;

namespace GatewayApi.Telemetry
{
    public class TelemetryFilter : IActionFilter, IResultFilter, IExceptionFilter
    {
        private readonly ITelemetryService _telemetryService;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();


        public TelemetryFilter(ITelemetryService telemetryService)
        {
            _telemetryService = telemetryService;
        }

        /// <summary>
        /// Add a random delay for more interesting metric visualization
        /// </summary>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Thread.Sleep(new Random().Next(0, 1000));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        /// <summary>
        /// Write a metric for each exception result.  
        /// Real status code isn't available at this point in the pipeline, so we set it to 500 here for now.
        /// </summary>
        public void OnException(ExceptionContext context)
        {
            var contextInfo = context.GetFilterContextInfo();
            contextInfo.StatusCode = (int)HttpStatusCode.InternalServerError;
            
            _telemetryService.LogHttpResponseMetrics(contextInfo,_stopwatch.ElapsedMilliseconds);
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            
        }

        /// <summary>
        /// Write a metric on each non-exception result
        /// </summary>
        public void OnResultExecuted(ResultExecutedContext context)
        {
            var contextInfo = context.GetFilterContextInfo();
            _telemetryService.LogHttpResponseMetrics(contextInfo, _stopwatch.ElapsedMilliseconds);
        }
    }
}
