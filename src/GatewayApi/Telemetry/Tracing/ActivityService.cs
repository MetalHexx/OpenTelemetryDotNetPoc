using System.Diagnostics;
using static GatewayApi.Telemetry.Constants.TelemetryConstants;

namespace GatewayApi.Telemetry.Tracing
{

    public class ActivityService : IActivityService
    {
        public Activity? StartActivity(string methodName, string className, string description)
        {
            var activity = PocActivitySource.StartActivity($"{className}.{methodName}");
            activity?.SetTag(Class_Tag, className);
            activity?.SetTag(Method_Tag, methodName);

            if (!string.IsNullOrWhiteSpace(description))
            {
                activity?.AddEvent(new(description));
            }
            return activity;
        }
    }
}
