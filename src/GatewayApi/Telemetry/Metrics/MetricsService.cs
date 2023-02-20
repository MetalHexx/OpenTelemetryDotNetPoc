using System.Diagnostics.Metrics;
using GatewayApi.Telemetry.Filters;
using static GatewayApi.Telemetry.TelemetryConstants;

namespace GatewayApi.Telemetry.Metrics
{
    public class MetricsService : IMetricsService
    {
        private Histogram<long> _httpRequestHistogram;
        public MetricsService()
        {
            _httpRequestHistogram = PocMeter.CreateHistogram<long>(
                name: "poc_http_response_histogram",
                unit: "ms",
                description: "Api response histogram metrics with durations");
        }

        public void LogHttpResponseMetrics(FilterContextInfo responseInfo, long duration)
        {
            LogHttpResponseMetrics(responseInfo.Route, responseInfo.ClassName, responseInfo.MethodName, responseInfo.StatusCode, duration);
        }

        public void LogHttpResponseMetrics(string route, string className, string method, int statusCode, long duration)
        {
            _httpRequestHistogram.Record(duration,
                new(Route_Tag, route),
                new(Class_Tag, className),
                new(Method_Tag, method),
                new(Http_Status_Code_Tag, statusCode.ToString()));
        }
    }
}
