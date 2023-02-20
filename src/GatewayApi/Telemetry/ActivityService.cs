﻿using System.Diagnostics;
using static GatewayApi.Telemetry.TelemetryConstants;

namespace GatewayApi.Telemetry
{
    public interface IActivityService
    {
        Activity? StartActivity(string methodName, string className);
    }

    public class ActivityService : IActivityService
    {
        public Activity? StartActivity(string methodName, string className)
        {
            var activity = PocActivitySource.StartActivity($"{className}.{methodName}");
            activity?.SetTag(Class_Tag, className);
            activity?.SetTag(Method_Tag, methodName);

            return activity;
        }
    }
}
