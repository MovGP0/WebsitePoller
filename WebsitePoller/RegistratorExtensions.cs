using System;
using DryIoc;
using NodaTime;
using WebsitePoller.Workflow;

namespace WebsitePoller
{
    public static class RegistratorExtensions
    {
        public static IRegistrator SetupDependencies(this IRegistrator registrator)
        {
            registrator.Register<IIntervallCalculator, IntervallCalculator>();
            registrator.Register<IPolicyFactory, PolicyFactory>();
            registrator.Register<ITownCrier, TownCrier>();
            registrator.RegisterDelegate<IClock>(r => SystemClock.Instance);
            registrator.Register<ITownCrierFactory, TownCrierFactory>();
            registrator.RegisterDelegate<Func<ITownCrier>>(r => () => r.Resolve<ITownCrier>());
            registrator.Register<IExecuteWorkFlowCommand, ExecuteWorkFlowCommand>();
            return registrator;
        }
    }
}