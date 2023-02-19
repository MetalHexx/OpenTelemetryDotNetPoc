using System.Net;

namespace GatewayApi.Telemetry
{
    public interface ITelemetryService
    {
        void LogHttpResponseMetrics(string route, string className, string method, int statusCode, long duration);
        void LogHttpResponseMetrics(FilterContextInfo responseInfo, long duration);
    }
}