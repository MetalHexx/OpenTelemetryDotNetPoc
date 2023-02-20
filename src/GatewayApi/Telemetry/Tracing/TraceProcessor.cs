using System.Diagnostics;
using OpenTelemetry;
using static GatewayApi.Telemetry.Constants.TelemetryConstants;

namespace GatewayApi.Telemetry.Tracing
{
    /// <summary>
    /// Trace processor to intercept and enrich traces
    /// </summary>
    public class TraceProcessor : BaseProcessor<Activity>
    {
        /// <summary>
        /// Jaeger seems to be missing the trace / span id in the visualizations. 
        /// So I'm adding them as additional tags by overriding the OnStart
        /// </summary>
        /// <param name="data"></param>
        public override void OnStart(Activity data)
        {
            data.SetTag(Trace_Tag, data.TraceId);
            data.SetTag(Span_Tag, data.SpanId);
            base.OnStart(data);
        }
    }
}
