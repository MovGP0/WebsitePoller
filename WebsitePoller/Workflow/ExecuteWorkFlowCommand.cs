﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Serilog;
using WebsitePoller.Entities;
using WebsitePoller.FormRegistrator;
using WebsitePoller.Parser;
using WebsitePoller.Setting;

namespace WebsitePoller.Workflow
{
    public sealed class ExecuteWorkFlowCommand : IExecuteWorkFlowCommand
    {
        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext<ExecuteWorkFlowCommand>();

        [NotNull]
        private IWebsiteDownloader WebsiteDownloader { get; }

        [NotNull]
        private IEqualityComparer<HtmlDocument> HtmlDocumentComparer { get; }
        
        [NotNull]
        private SettingsManager SettingsManager { get; }

        [NotNull]
        private ISettingsLoader SettingsLoader { get; }

        [NotNull]
        private IAltbauWohnungenParser AltbauWohnungenParser { get; }

        [NotNull]
        private IAltbauWohnungenFilter AltbauWohnungenFilter { get; }

        [NotNull]
        private IFormRegistrator FormRegistrator { get; }

        [NotNull]
        private INotifyHelper NotifyHelper { get; }

        public ExecuteWorkFlowCommand(
            [NotNull] IWebsiteDownloader websiteDownloader,
            [NotNull] IEqualityComparer<HtmlDocument> htmlDocumentComparer,
            [NotNull] SettingsManager settingsManager,
            [NotNull] ISettingsLoader settingsLoader,
            [NotNull] IAltbauWohnungenParser altbauWohnungenParser,
            [NotNull] IAltbauWohnungenFilter altbauWohnungenFilter,
            [NotNull] IFormRegistrator formRegistrator, 
            [NotNull] INotifyHelper notifyHelper)
        {
            NotifyHelper = notifyHelper ?? throw new ArgumentNullException(nameof(notifyHelper));
            AltbauWohnungenParser = altbauWohnungenParser ?? throw new ArgumentNullException(nameof(altbauWohnungenParser));
            AltbauWohnungenFilter = altbauWohnungenFilter ?? throw new ArgumentNullException(nameof(altbauWohnungenFilter));
            FormRegistrator = formRegistrator ?? throw new ArgumentNullException(nameof(formRegistrator));
            WebsiteDownloader = websiteDownloader ?? throw new ArgumentNullException(nameof(websiteDownloader));
            HtmlDocumentComparer = htmlDocumentComparer ?? throw new ArgumentNullException(nameof(htmlDocumentComparer));
            SettingsManager = settingsManager ?? throw new ArgumentNullException(nameof(settingsManager));
            SettingsLoader = settingsLoader ?? throw new ArgumentNullException(nameof(settingsLoader));
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var timeout = TimeSpan.FromMinutes(5);
            using (var source = new CancellationTokenSource(timeout))
            {
                var token = source.Token;
                ExecuteAsync(token).Wait(token);
            }
        }

        // TODO: split into smaller commands
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var settings = LoadSettings();
            var url = settings.Url;
            var applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var targetPath = Path.Combine(applicationDataPath, "altbau.xhtml");
            
            var webpage = await WebsiteDownloader.GetWebsiteOrNullWithPolicyAndLoggingAsync(url, targetPath, cancellationToken);
            if (webpage == null) return;

            if (File.Exists(targetPath))
            {
                var cachedWebpage = await HtmlDocumentFactory.FromFileAsync(targetPath, cancellationToken);

                var areEqual = HtmlDocumentComparer.Equals(webpage, cachedWebpage);
                if (areEqual)
                {
                    Log.Verbose("Website has not changed");
                    return;
                }

                NotifyHelper.ShowNotificationThatWebsiteHasChanged("Website has changed", url);

                var offers = AltbauWohnungenParser.ParseAltbauWohnungenDocumentWithLogging(webpage);
                var interestingOffers = AltbauWohnungenFilter.Filter(offers);

                var postedHrefsPath = Path.Combine(applicationDataPath, "hrefs.txt");
                var hrefs = FileHelper.GetFileLines(postedHrefsPath).ToArray();

                var newOffers = interestingOffers.Where(o => !hrefs.Contains(o.Href)).ToArray();

                var domain = settings.Url.GetDomain();
                foreach (var newOffer in newOffers)
                {
                    await FormRegistrator.PostRegistrationWithPolicyAndLoggingAsync(domain, newOffer.Href, cancellationToken);
                    NotifyHelper.ShowNotificationThatRegisteredForOffer(newOffer);
                }

                var lines = newOffers.Select(o => o.Href);
                await FileHelper.CreateFileIfNotExistsAndAppendLinesToFileAsync(postedHrefsPath, lines);
                
                File.Delete(targetPath);
            }

            await webpage.SaveHtmlDocumentToFileWithLoggingAsync(targetPath, cancellationToken);
        }

        private Settings LoadSettings()
        {
            SettingsLoader.UpdateSettings();
            var settings = SettingsManager.Settings;
            if (settings == null) throw new InvalidOperationException($"{nameof(SettingsManager.Settings)} was null.");
            return settings;
        }
    }
}