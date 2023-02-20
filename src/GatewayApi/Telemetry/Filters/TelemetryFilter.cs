using GatewayApi.Telemetry.Metrics;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Net;

namespace GatewayApi.Telemetry.Filters
{
    public class MetricsFilter : IActionFilter, IResultFilter, IExceptionFilter
    {
        private readonly IMetricsService _metricService;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public MetricsFilter(IMetricsService metricService)
        {
            _metricService = metricService;
        }

        /// <summary>
        /// Add a random delay for more interesting metric visualization
        /// </summary>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var randomSleep = new Random().Next(0, 1000);
            Thread.Sleep(new Random().Next(0, 1000));

            if (randomSleep > 800)
            {
                throw new Exception("Throwing an exception over 800ms to simulate a 500 error");
            }
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

            _metricService.LogHttpResponseMetrics(contextInfo, _stopwatch.ElapsedMilliseconds);
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
            _metricService.LogHttpResponseMetrics(contextInfo, _stopwatch.ElapsedMilliseconds);
        }
    }
}
