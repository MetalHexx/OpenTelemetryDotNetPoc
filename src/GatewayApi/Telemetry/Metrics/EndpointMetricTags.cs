namespace GatewayApi.Telemetry.Metrics
{
    public record EndpointMetricTags(int StatusCode, string RouteTemplate, string ClassName, string ClassMethodName, string ServiceName, string Environment);
}
