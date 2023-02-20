using System.Diagnostics;
using static GatewayApi.Telemetry.Constants.TelemetryConstants;

namespace GatewayApi.Telemetry.Tracing
{
    public class ActivityService : IActivityService
    {
        public Activity? StartActivity(ActivityTags tags)
        {
            var activity = PocActivitySource.StartActivity($"{tags.ClassName}.{tags.MethodName}");
            activity?.SetTag(Class_Tag, tags.ClassName);
            activity?.SetTag(Method_Tag, tags.MethodName);

            if (!string.IsNullOrWhiteSpace(tags.Description))
            {
                activity?.AddEvent(new(tags.Description));
            }
            return activity;
        }
    }
}
