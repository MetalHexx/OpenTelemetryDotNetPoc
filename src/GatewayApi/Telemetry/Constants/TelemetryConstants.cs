﻿using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace GatewayApi.Telemetry.Constants
{
    public static class TelemetryConstants
    {
        public const string App_Settings_Log_Level = "Logging:LogLevel:Default";
        public const string Service_Name = "poc-api-gateway";

        public static readonly Meter PocMeter = new(Service_Name);
        public static ActivitySource PocActivitySource = new(Service_Name);

        public const string Service_Name_Tag  = "service_name";
        public const string Environment_Tag = "environment";
        public const string Route_Template_Tag = "route_template";
        public const string Class_Tag = "class";
        public const string Class_Method_Tag = "class_method";        
        public const string Trace_Tag = "trace_id";
        public const string Span_Tag = "span_id";
        public const string Http_Status_Code_Tag = "http_status_code";
        public const string Unknown = "Unknown";
    }
}
