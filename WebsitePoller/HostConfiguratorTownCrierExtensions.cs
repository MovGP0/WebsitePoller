using System;
using Serilog;
using Topshelf;
using Topshelf.HostConfigurators;

namespace WebsitePoller
{
    public static class HostConfiguratorTownCrierExtensions
    {
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
                    tc.Start();
                });
                s.WhenStopped(tc =>
                {
                    Log.Information("Stopping...");
                    tc.Stop();
                });
                s.WhenPaused(tc =>
                {
                    Log.Information("Pausing...");
                    tc.Stop();
                });

                s.WhenContinued(tc =>
                {
                    Log.Information("Continuing...");
                    tc.Start();
                });

                s.WhenShutdown(tc =>
                {
                    Log.Information("Shutting down...");
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