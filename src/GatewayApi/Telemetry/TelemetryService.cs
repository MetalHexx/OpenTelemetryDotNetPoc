using System.Diagnostics.Metrics;
using static GatewayApi.Telemetry.TelemetryConstants;

namespace GatewayApi.Telemetry
{
    public class TelemetryService : ITelemetryService
    {
        private Histogram<long> _httpRequestHistogram;
        public TelemetryService()
        {
            _httpRequestHistogram = PocMeter.CreateHistogram<long>(
                name: "poc_http_response_histogram",
                unit: "ms",
                description: "Requests to api with durations");
        }

        public void LogHttpResponsequestMetric(ActionFilterResponseInfo responseInfo, long duration)
        {
            LogHttpResponsequestMetric(responseInfo.Route, responseInfo.ClassName, responseInfo.MethodName, responseInfo.StatusCode, duration);
        }

        public void LogHttpResponsequestMetric(string route, string className, string method, int statusCode, long duration)
        {
            _httpRequestHistogram.Record(duration,
                new KeyValuePair<string, object?>("route", route),
                new KeyValuePair<string, object?>("class", className),
                new KeyValuePair<string, object?>("method", method),
                new KeyValuePair<string, object?>("http_status_code", statusCode.ToString()));
        }
    }
}
