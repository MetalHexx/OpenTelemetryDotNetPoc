namespace GatewayApi.Telemetry.Metrics
{
    public record EndpointMetricTags(int StatusCode, string Route, string ClassName, string MethodName);
}
