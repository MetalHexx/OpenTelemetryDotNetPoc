using System.Diagnostics;

namespace GatewayApi.Telemetry.Tracing
{
    public interface IActivityService
    {
        Activity? StartActivity(string className, string methodName, string description);
    }
}
