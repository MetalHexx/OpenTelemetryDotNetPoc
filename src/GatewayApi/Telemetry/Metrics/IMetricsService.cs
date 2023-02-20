using System.Net;
using GatewayApi.Telemetry.Filters;

namespace GatewayApi.Telemetry.Metrics
{
    public interface IMetricsService
    {
        void LogHttpResponseMetrics(string route, string className, string method, int statusCode, long duration);
        void LogHttpResponseMetrics(FilterContextInfo responseInfo, long duration);
    }
}