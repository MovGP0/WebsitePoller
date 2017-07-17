using System;
using System.Configuration;
using System.Text;
using JetBrains.Annotations;
using Serilog;
using Serilog.Events;

namespace WebsitePoller
{
    public static class LoggerHelper
    {
        [NotNull]
        public static ILogger SetupLogger(LogEventLevel level = LogEventLevel.Verbose)
        {
            var instrumentionKey = ConfigurationManager.AppSettings["InstrumentationKey"];

            return new LoggerConfiguration()
                .WriteTo.EventLog(Constants.ServiceName, Constants.LogName, Constants.MachineName, false, restrictedToMinimumLevel: LogEventLevel.Warning)
                .WriteTo.LiterateConsole(level)
                .WriteTo.ApplicationInsightsEvents(instrumentionKey, LogEventLevel.Information)
                .CreateLogger();
        }
        
        [NotNull]
        public static string GetLogLevels()
        {
            var sb = new StringBuilder();
            var isFirst = true;
            foreach (var level in Enum.GetValues(typeof(LogEventLevel)))
            {
                if (isFirst)
                {
                    sb.AppendFormat($"{level}");
                    isFirst = false;
                    continue;
                }

                sb.AppendFormat($", {level}");
            }
            return sb.ToString();
        }
    }
}