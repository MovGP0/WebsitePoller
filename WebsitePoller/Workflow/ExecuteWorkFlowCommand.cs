using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Serilog;

namespace WebsitePoller.Workflow
{
    public sealed class ExecuteWorkFlowCommand : IExecuteWorkFlowCommand
    {
        private static ILogger Log => Serilog.Log.ForContext<ExecuteWorkFlowCommand>();

        private IWebsiteDownloader WebsiteDownloader { get; }
        private IEqualityComparer<HtmlDocument> HtmlDocumentComparer { get; }
        private INotifier Notifier { get; }
        private Settings Settings { get; }
        private ISettingsLoader SettingsLoader { get; }

        public ExecuteWorkFlowCommand(
            IWebsiteDownloader websiteDownloader,
            IEqualityComparer<HtmlDocument> htmlDocumentComparer, 
            INotifier notifier, 
            Settings settings, 
            ISettingsLoader settingsLoader)
        {
            WebsiteDownloader = websiteDownloader;
            HtmlDocumentComparer = htmlDocumentComparer;
            Notifier = notifier;
            Settings = settings;
            SettingsLoader = settingsLoader;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var timeout = TimeSpan.FromMinutes(1);
            using (var source = new CancellationTokenSource(timeout))
            {
                var token = source.Token;
                ExecuteAsync(token).Wait(token);
            }
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "altbau.xhtml");
            
            SettingsLoader.UpdateSettings();

            var webpage = await WebsiteDownloader.GetWebsiteOrNullWithPolicyAndLoggingAsync(Settings.Url, targetPath, cancellationToken);
            if (webpage == null) return;
            
            if (File.Exists(targetPath))
            {
                var cachedWebpage = await HtmlDocumentFactory.FromFileAsync(targetPath, cancellationToken);
                
                ShowNotificationIfWebsitesDiffer(webpage, cachedWebpage, "Website has changed", Settings.Url);
                File.Delete(targetPath);
            }
            
            await webpage.SaveHtmlDocumentToFileWithLoggingAsync(targetPath, cancellationToken);
        }

        private void ShowNotificationIfWebsitesDiffer(HtmlDocument webpage, HtmlDocument cachedWebpage, string message, Uri uri)
        {
            var areEqual = HtmlDocumentComparer.Equals(webpage, cachedWebpage);
            if (areEqual)
            {
                Log.Verbose("Website has not changed");
                return;
            }

            Log.Verbose("Informing user that site has changed");
            Notifier.Notify(message, uri);
        }
    }
}