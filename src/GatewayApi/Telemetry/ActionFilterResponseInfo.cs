namespace GatewayApi.Telemetry
{
    public class ActionFilterResponseInfo
    {
        public string Route { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string MethodName { get; set; } = string.Empty;
        public int StatusCode { get; set; }

    }
}
