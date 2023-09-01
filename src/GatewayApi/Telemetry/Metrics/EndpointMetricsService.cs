using Microsoft.AspNetCore.Routing;
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
            _endpointResponseHistogram.Record(duration,
                new(Route_Template_Tag, responseInfo.RouteTemplate),
                new(Class_Tag, responseInfo.ClassName),
                new(Class_Method_Tag, responseInfo.ClassMethodName),
                new(Http_Status_Code_Tag, responseInfo.StatusCode),
                new(Service_Name_Tag, Service_Name));
        }
    }
}
