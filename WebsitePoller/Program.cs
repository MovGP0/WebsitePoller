using System;
using DryIoc;
using JetBrains.Annotations;
using Serilog;
using Serilog.Events;
using WebsitePoller.Setting;

namespace WebsitePoller
{
    internal class Program
    {
        private const string ServiceName = "WebsitePoller";
        private const string LogName = "Application";

        [NotNull]
        private static readonly string MachineName = Environment.MachineName;

        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext<Program>();

        private static void Main()
        {
            // Logging
            Serilog.Log.Logger = SetupLogger();

            // Dependency Injection
            var resolver = SetupDependencyResolver();
            ITownCrier TownCrierFactory()
            {
                var factory = resolver.Resolve<ITownCrierFactory>();
                return factory.Invoke();
            }

            // Settings
            var settingsLoader = resolver.Resolve<ISettingsLoader>();
            settingsLoader.UpdateSettings();
            
            ConfigureService.Configure(ServiceName, LogName, MachineName, TownCrierFactory);
        }

        [NotNull]
        private static ILogger SetupLogger()
        {
            return new LoggerConfiguration()
                .WriteTo.EventLog(ServiceName, LogName, MachineName, false, restrictedToMinimumLevel: LogEventLevel.Warning)
                .WriteTo.LiterateConsole()
                .CreateLogger();
        }

        [NotNull]
        private static IResolver SetupDependencyResolver()
        {
            return (Container)new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient())
                .SetupDependencies()
                .SetupMappings();
        }
    }
}
