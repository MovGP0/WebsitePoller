using System;
using Serilog;
using Topshelf;
using JetBrains.Annotations;

namespace WebsitePoller
{
    public static class ConfigureService
    {
        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext(typeof(ConfigureService));

        public static void Configure(string ServiceName, string MachineName, string LogName, Func<ITownCrier> TownCrierFactory)
        {
            HostFactory.Run(config =>
            {
                config.SetDescription("Background service that polls websites for changes in the background.");
                config.SetDisplayName("Website Poller");
                config.SetServiceName(ServiceName);
                config.ConfigureTownCrierService(TownCrierFactory);
                config.ConfigureLogging(ServiceName, MachineName, LogName);
                config.OnException(e =>
                {
                    Log.Error(e, e.Message);
                });
            });
        }
    } 
}