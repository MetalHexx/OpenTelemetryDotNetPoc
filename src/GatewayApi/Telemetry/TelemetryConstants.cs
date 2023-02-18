using System.Diagnostics.Metrics;

namespace GatewayApi.Telemetry
{
    public static class TelemetryConstants
    {
        public const string AppSource = "poc-api-gateway";

        public static readonly Meter PocMeter = new(AppSource);
    }
}
