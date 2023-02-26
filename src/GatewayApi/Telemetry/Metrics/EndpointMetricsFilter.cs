using GatewayApi.Telemetry.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace GatewayApi.Telemetry.Metrics
{
    public class EndpointMetricsFilter : IActionFilter, IResultFilter, IExceptionFilter
    {
        private readonly IEndpointMetricsService _metricService;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public EndpointMetricsFilter(IEndpointMetricsService metricService)
        {
            _metricService = metricService;
        }

        /// <summary>
        /// Adding a random delay and/or exception for more interesting metric generation
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

        public void OnActionExecuted(ActionExecutedContext context) { }

        /// <summary>
        /// Recording a metric for each exception result.  
        /// </summary>
        public void OnException(ExceptionContext context)
        {   
            var tags = context.GetEndpointMetricTags() with 
            { 
                StatusCode = StatusCodes.Status500InternalServerError 
            };
            _metricService.RecordEndpointMetrics(tags, _stopwatch.ElapsedMilliseconds);
            
            //TODO: Create a clean response envelope without stack trace
        }

        public void OnResultExecuting(ResultExecutingContext context) { }

        /// <summary>
        /// Record a metric on each non-exception result
        /// </summary>
        public void OnResultExecuted(ResultExecutedContext context)
        {
            var tags = context.GetEndpointMetricTags();
            _metricService.RecordEndpointMetrics(tags, _stopwatch.ElapsedMilliseconds);
        }
    }
}
