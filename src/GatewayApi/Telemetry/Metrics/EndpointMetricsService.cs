using System.Diagnostics.Metrics;
using static GatewayApi.Telemetry.Constants.TelemetryConstants;

namespace GatewayApi.Telemetry.Metrics
{
    public class EndpointMetricsService : IEndpointMetricsService
    {
        private Histogram<long> _endpointResponseHistogram;
        public EndpointMetricsService()
        {
            _endpointResponseHistogram = PocMeter.CreateHistogram<long>(
                name: "http_response_histogram_ms",
                unit: "ms",
                description: "Api response histogram metrics with durations");
        }

        public void RecordEndpointMetrics(EndpointMetricTags responseInfo, long duration)
        {
            RecordEndpointMetrics(responseInfo.RouteTemplate, responseInfo.ClassName, responseInfo.ClassMethodName, responseInfo.StatusCode, duration);
        }

        public void RecordEndpointMetrics(string route, string className, string method, int statusCode, long duration)
        {
            _endpointResponseHistogram.Record(duration,
                new(Route_Template_Tag, route),
                new(Class_Tag, className),
                new(Class_Method_Tag, method),
                new(Http_Status_Code_Tag, statusCode.ToString()));
        }
    }
}
