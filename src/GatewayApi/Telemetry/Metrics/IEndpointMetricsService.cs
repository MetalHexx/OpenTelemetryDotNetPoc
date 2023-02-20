namespace GatewayApi.Telemetry.Metrics
{
    public interface IEndpointMetricsService
    {
        void RecordEndpointMetrics(string route, string className, string method, int statusCode, long duration);
        void RecordEndpointMetrics(EndpointMetricTags responseInfo, long duration);
    }
}