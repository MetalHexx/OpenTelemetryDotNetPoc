namespace GatewayApi.Telemetry.Filters
{
    public class FilterContextInfo
    {
        public string Route { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string MethodName { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }
}
