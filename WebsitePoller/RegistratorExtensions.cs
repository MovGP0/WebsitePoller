﻿using System;
using System.Collections.Generic;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using DryIoc;
using HtmlAgilityPack;
using NodaTime;
using RestSharp;
using WebsitePoller.FormRegistrator;
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
            registrator.RegisterDelegate<Func<Uri, IRestClient>>(r => uri => new RestClient(uri){ Encoding = Encoding.UTF8 });
            registrator.Register<IToastNotifier, ToastNotifierWrapper>();
            registrator.RegisterDelegate(r => ToastNotificationManager.CreateToastNotifier("Website Poller"));
            registrator.Register<IAddressFieldParser, AddressFieldParser>();
            registrator.Register<IAltbauWohnungenFilter, AltbauWohnungenFilter>();
            registrator.Register<IAltbauWohnungenRowParser, AltbauWohnungenRowParser>();
            registrator.RegisterDelegate<Func<XmlDocument, ToastNotification>>(r => x => new ToastNotification(x));
            registrator.Register<INotifyHelper, NotifyHelper>();
            registrator.Register<IFormRegistrator, FormRegistrator.FormRegistrator>();
            return registrator;
        }
    }
}