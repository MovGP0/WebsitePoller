using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Serilog;
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
        private INotifier Notifier { get; }

        [NotNull]
        private SettingsManager SettingsManager { get; }

        [NotNull]
        private ISettingsLoader SettingsLoader { get; }

        public ExecuteWorkFlowCommand(
            [NotNull]IWebsiteDownloader websiteDownloader,
            [NotNull]IEqualityComparer<HtmlDocument> htmlDocumentComparer,
            [NotNull]INotifier notifier,
            [NotNull]SettingsManager settingsManager,
            [NotNull]ISettingsLoader settingsLoader)
        {
            WebsiteDownloader = websiteDownloader ?? throw new ArgumentNullException(nameof(websiteDownloader));
            HtmlDocumentComparer = htmlDocumentComparer ?? throw new ArgumentNullException(nameof(htmlDocumentComparer));
            Notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
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
            var settings = SettingsManager.Settings;
            if(settings == null) throw new InvalidOperationException($"{nameof(SettingsManager.Settings)} was null.");

            var url = settings.Url;

            var targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "altbau.xhtml");
            
            SettingsLoader.UpdateSettings();

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

                ShowNotificationThatWebsiteHasChanged("Website has changed", url);

                // TODO: 
                // get offers from website
                // filter offers
                // load checksums of posted offers
                // filter by checksum
                // post for any remaining offer
                // add checksums to file

                File.Delete(targetPath);
            }
            
            await webpage.SaveHtmlDocumentToFileWithLoggingAsync(targetPath, cancellationToken);
        }

        private void ShowNotificationThatWebsiteHasChanged(string message, Uri uri)
        {
            Log.Verbose("Informing user that site has changed");
            Notifier.Notify(message, uri);
        }
        
    }
}