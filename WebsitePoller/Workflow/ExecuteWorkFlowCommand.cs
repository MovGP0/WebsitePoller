using System;
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
        private IFileContentComparer FileContentComparer { get; }
        private INotifier Notifier { get; }
        private Settings Settings { get; }
        private ISettingsLoader SettingsLoader { get; }

        public ExecuteWorkFlowCommand(
            IWebsiteDownloader websiteDownloader, 
            IFileContentComparer fileContentComparer, 
            INotifier notifier, 
            Settings settings, 
            ISettingsLoader settingsLoader)
        {
            WebsiteDownloader = websiteDownloader;
            FileContentComparer = fileContentComparer;
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
            var cachedFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "altbau.copy.xhtml");

            SettingsLoader.UpdateSettings();

            var webpage = await GetWebpageOrNull(Settings.Url, targetPath, cancellationToken);
            if (webpage == null) return;

            if(File.Exists(targetPath)) File.Delete(targetPath); 
            webpage.Save(targetPath);

            if (File.Exists(cachedFilePath))
            {
                ShowNotificationIfWebsitesDiffer(targetPath, cachedFilePath, "Website has changed", Settings.Url);
                File.Delete(cachedFilePath);
            }
            
            File.Move(targetPath, cachedFilePath);
        }

        private async Task<HtmlDocument> GetWebpageOrNull(Uri uri, string targetPath, CancellationToken cancellationToken)
        {
            return await WebsiteDownloader.GetWebsiteOrNullWithPolicyAsync(uri, targetPath, cancellationToken);
        }

        private void ShowNotificationIfWebsitesDiffer(string targetPath, string cachedFilePath, string message, Uri uri)
        {
            var areEqual = FileContentComparer.Equals(targetPath, cachedFilePath);
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