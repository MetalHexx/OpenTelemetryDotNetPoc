namespace GatewayApi.Telemetry.Extensions
{
    public static class LogLevelExtensions
    {
        public static LogLevel ToLogLevel(this string logLevel)
        {
            return logLevel switch
            {
                "Trace" => LogLevel.Trace,
                "Debug" => LogLevel.Debug,
                "Information" => LogLevel.Information,
                "Warning" => LogLevel.Warning,
                "Error" => LogLevel.Error,
                "Critical" => LogLevel.Critical,
                "None" => LogLevel.None,
                _ => LogLevel.None,
            };
        }

    }
}
