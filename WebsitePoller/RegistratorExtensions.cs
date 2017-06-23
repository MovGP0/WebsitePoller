using System;
using System.Collections.Generic;
using AutoMapper;
using DryIoc;
using HtmlAgilityPack;
using NodaTime;
using WebsitePoller.Parser;
using WebsitePoller.Setting;
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
            registrator.Register<ISettingsLoader, SettingsLoader>();
            registrator.Register<IWebsiteDownloader, WebsiteDownloader>();
            registrator.Register<SettingsManager>(Reuse.Singleton);
            registrator.Register<IFileContentComparer, FileContentComparer>();
            registrator.Register<INotifier, Notifier>();
            registrator.Register<IEqualityComparer<HtmlDocument>, HtmlDocumentComparer>();
            registrator.Register<IAltbauWohnungenParser, AltbauWohnungenParser>();
            return registrator;
        }

        public static IRegistrator SetupMappings(this IRegistrator registrator)
        {
            registrator.RegisterDelegate(r => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }));
            registrator.RegisterDelegate<IMapper>(r => new Mapper(r.Resolve<MapperConfiguration>()), Reuse.Singleton);
            return registrator;
        }
    }
}