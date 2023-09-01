using System.Diagnostics;
using static GatewayApi.Telemetry.Constants.TelemetryConstants;

namespace GatewayApi.Telemetry.Tracing
{
    public class ActivityService : IActivityService
    {
        private readonly IWebHostEnvironment _env;
        public ActivityService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public Activity? StartActivity(ActivityTags tags)
        {
            var activity = PocActivitySource.StartActivity($"{tags.ClassName}.{tags.ClassMethodName}");
            activity?.SetTag(Class_Tag, tags.ClassName);
            activity?.SetTag(Class_Method_Tag, tags.ClassMethodName);
            activity?.SetTag(Service_Name_Tag, Service_Name);  //I shouldn't need this duplication...however makes jaeger --> loki correlation work in grafana.
            activity?.SetTag(Environment_Tag, _env.EnvironmentName);

            if (!string.IsNullOrWhiteSpace(tags.Description))
            {
                activity?.AddEvent(new(tags.Description));
            }
            return activity;
        }
    }
}
