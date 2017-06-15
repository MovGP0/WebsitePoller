using System.Diagnostics;
using Topshelf;
using Topshelf.HostConfigurators;

namespace WebsitePoller
{
    public static class HostConfiguratorLoggingExtensions
    {
        public static HostConfigurator ConfigureLogging(this HostConfigurator config, string serviceName, string machineName, string logName)
        {
            config.DependsOnEventLog();
            config.BeforeInstall(settings => CreateEventLogSource(serviceName, machineName, logName));
            config.BeforeUninstall(() => DeleteEventLogSource(serviceName, machineName));
            config.UseSerilog();
            return config;
        }

        private static void DeleteEventLogSource(string serviceName, string machineName)
        {
            if (!EventLog.SourceExists(serviceName)) return;

            var permission = new EventLogPermission(EventLogPermissionAccess.Administer, machineName);
            permission.Assert();

            EventLog.DeleteEventSource(serviceName, machineName);
        }

        private static void CreateEventLogSource(string serviceName, string machineName, string logName)
        {
            if (EventLog.SourceExists(serviceName)) return;

            var permission = new EventLogPermission(EventLogPermissionAccess.Administer, machineName);
            permission.Assert();

            var eventSourceCreationData = new EventSourceCreationData(serviceName, logName)
            {
                MachineName = machineName
            };
            EventLog.CreateEventSource(eventSourceCreationData);
        }
    }
}