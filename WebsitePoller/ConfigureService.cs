using System;
using Serilog;
using Topshelf;
using JetBrains.Annotations;
using Serilog.Events;
using WebsitePoller.Setting;

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
                config.AddCommandLineDefinition("loglevel", lvl =>
                {
                    Log.Debug($"Parsing to log level '{lvl}'.");
                    if (Enum.TryParse(lvl, true, out LogEventLevel logEventLevel))
                    {
                        Log.Information($"Switching to log level '{logEventLevel}'.");
                        Serilog.Log.Logger = LoggerHelper.SetupLogger(logEventLevel);
                        return;
                    }

                    var levels = LoggerHelper.GetLogLevels();
                    Log.Warning($"Could not switch to log level '{lvl}'. Possible values are {levels}.");
                });
                config.AddCommandLineSwitch("noinit", isSkipInit => SettingsManager.IsFirstRun = !isSkipInit);
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