using System;
using Microsoft.Win32;
using Serilog;
using Topshelf;
using Topshelf.HostConfigurators;

namespace WebsitePoller
{
    public static class HostConfiguratorTownCrierExtensions
    {
        private static ILogger Log => Serilog.Log.ForContext(typeof(HostConfiguratorTownCrierExtensions));

        private static ITownCrier TownCrier { get; set; }

        private static void OnPowerChange(object s, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    Log.Information("Resuming");
                    TownCrier.Start();
                    break;
                case PowerModes.Suspend:
                    Log.Information("Suspending");
                    TownCrier.Stop();
                    break;
                case PowerModes.StatusChange:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void ConfigureTownCrierService(this HostConfigurator config, Func<ITownCrier> townCrierFactory)
        {
            config.EnableServiceRecovery(rc =>
            {
                rc.RestartService(1);
                rc.SetResetPeriod(7);
            });

            config.EnablePauseAndContinue();
            config.EnableShutdown();

            config.Service<ITownCrier>(s =>
            {
                s.ConstructUsing(name => townCrierFactory());
                s.WhenStarted(tc =>
                {
                    Log.Information("Starting...");
                    TownCrier = tc;
                    SystemEvents.PowerModeChanged += OnPowerChange;
                    tc.Start();
                });
                s.WhenStopped(tc =>
                {
                    Log.Information("Stopping...");
                    TownCrier = tc;
                    SystemEvents.PowerModeChanged -= OnPowerChange;
                    tc.Stop();
                });
                s.WhenPaused(tc =>
                {
                    Log.Information("Pausing...");
                    TownCrier = tc;
                    SystemEvents.PowerModeChanged -= OnPowerChange;
                    tc.Stop();
                });

                s.WhenContinued(tc =>
                {
                    Log.Information("Continuing...");
                    TownCrier = tc;
                    SystemEvents.PowerModeChanged += OnPowerChange;
                    tc.Start();
                });

                s.WhenShutdown(tc =>
                {
                    Log.Information("Shutting down...");
                    TownCrier = tc;
                    SystemEvents.PowerModeChanged -= OnPowerChange;
                    tc.Stop();
                });

                s.WhenPowerEvent((service, args) =>
                {
                    Log.Information($"PowerEvent ({args.EventCode})");
                    return false;
                });
            });

            config.RunAsLocalSystem();
            config.StartAutomatically();
        }
    }
}