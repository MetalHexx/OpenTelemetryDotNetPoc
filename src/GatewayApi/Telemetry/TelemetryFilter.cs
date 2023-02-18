using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

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

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnException(ExceptionContext context)
        {
            _telemetryService.LogHttpResponsequestMetric(
                context.GetResponseInfo(),
                _stopwatch.ElapsedMilliseconds);
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            _telemetryService.LogHttpResponsequestMetric(
                context.GetResponseInfo(),
                _stopwatch.ElapsedMilliseconds);
        }
    }
}
