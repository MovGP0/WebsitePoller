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

        public static void Configure(string serviceName, string machineName, string logName, Func<ITownCrier> townCrierFactory)
        {
            HostFactory.Run(config =>
            {
                config.SetDescription("Background service that polls websites for changes in the background.");
                config.SetDisplayName("Website Poller");
                config.SetServiceName(serviceName);
                config.ConfigureTownCrierService(townCrierFactory);
                config.ConfigureLogging(serviceName, machineName, logName);
                config.OnException(e =>
                {
                    Log.Error(e, e.Message);
                });
            });
        }
    } 
}