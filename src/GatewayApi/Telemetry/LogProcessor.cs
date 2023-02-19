using OpenTelemetry;
using OpenTelemetry.Logs;

namespace GatewayApi.Telemetry
{
    /// <summary>
    /// Log processor to allow for log modification during the log lifecycle.
    /// </summary>
    public class LogProcessor : BaseProcessor<LogRecord>
    {
        /// <summary>
        /// You can customize the log record on start.
        /// </summary>
        public override void OnStart(LogRecord data)
        {
            Console.WriteLine("LogProcessor.OnStart()");
            base.OnStart(data);
        }

        /// <summary>
        /// You can customize the log record on end.
        /// </summary>
        public override void OnEnd(LogRecord data)
        {
            Console.WriteLine("LogProcessor.OnEnd()");
            base.OnEnd(data);
        }
    }
}
