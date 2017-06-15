using System;
using DryIoc;
using JetBrains.Annotations;
using Serilog;
using Serilog.Events;
using Topshelf;

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
            ITownCrier TownCrierFactory()
            {
                var factory = SetupDependencyResolver().Resolve<ITownCrierFactory>();
                return factory.Invoke();
            }

            HostFactory.Run(config =>
            {
                config.SetDescription("Backgronud service that polls websites for changes in the background.");
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

        [NotNull]
        private static ILogger SetupLogger()
        {
            return new LoggerConfiguration()
                .WriteTo.EventLog(ServiceName, LogName, MachineName, false, restrictedToMinimumLevel: LogEventLevel.Warning)
                .WriteTo.LiterateConsole(LogEventLevel.Information)
                .CreateLogger();
        }

        [NotNull]
        private static IResolver SetupDependencyResolver()
        {
            var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());
            container.SetupDependencies();
            return container;
        }
    }
}
