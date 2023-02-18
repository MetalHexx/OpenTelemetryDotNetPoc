using System.Net;

namespace GatewayApi.Telemetry
{
    public interface ITelemetryService
    {
        void LogHttpResponsequestMetric(string route, string className, string method, int statusCode, long duration);
        void LogHttpResponsequestMetric(ActionFilterResponseInfo responseInfo, long duration);
    }
}