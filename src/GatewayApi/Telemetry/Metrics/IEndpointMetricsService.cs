namespace GatewayApi.Telemetry.Metrics
{
    public interface IEndpointMetricsService
    {
        void RecordEndpointMetrics(EndpointMetricTags responseInfo, long duration);
    }
}