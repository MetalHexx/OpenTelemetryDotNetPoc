using System.Diagnostics;

namespace GatewayApi.Telemetry.Tracing
{
    public interface IActivityService
    {
        Activity? StartActivity(ActivityTags tags);
    }
}
