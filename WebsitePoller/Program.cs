using DryIoc;
using JetBrains.Annotations;
using Serilog;
using WebsitePoller.Mappings;
using WebsitePoller.Setting;
using WebsitePoller.Workflow;

namespace WebsitePoller
{
    internal class Program
    {
        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext<Program>();

        private static void Main()
        {
            Serilog.Log.Logger = LoggerHelper.SetupLogger();
            
            Log.Verbose("setting up dependency injection...");
            var resolver = SetupDependencyResolver();
            ITownCrier TownCrierFactory()
            {
                var factory = resolver.Resolve<ITownCrierFactory>();
                return factory.Invoke();
            }

            var applicationDataPath = FileHelper.GetApplicationDataPath();
            Log.Verbose($"Application data will be stored in '{applicationDataPath}'.", applicationDataPath);

            Log.Verbose("loading settings...");
            var settingsLoader = resolver.Resolve<ISettingsLoader>();
            settingsLoader.UpdateSettings();

            Log.Verbose("configuring service...");
            ConfigureService.Configure(Constants.ServiceName, Constants.LogName, Constants.MachineName, TownCrierFactory);
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
