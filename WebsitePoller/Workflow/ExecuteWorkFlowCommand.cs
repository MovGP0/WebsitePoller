using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WebsitePoller.Workflow
{
    public sealed class ExecuteWorkFlowCommand : IExecuteWorkFlowCommand
    {
        private IWebsiteDownloader WebsiteDownloader { get; }
        private IFileContentComparer FileContentComparer { get; }
        private INotifier Notifier { get; }

        public ExecuteWorkFlowCommand(
            IWebsiteDownloader websiteDownloader, 
            IFileContentComparer fileContentComparer, 
            INotifier notifier)
        {
            WebsiteDownloader = websiteDownloader;
            FileContentComparer = fileContentComparer;
            Notifier = notifier;
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
            var uri = new Uri("http://m.sozialbau.at/wohnungen/altbau/");
            var targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "altbau.xhtml");
            var cachedFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "altbau.copy.xhtml");
            
            await DownloadWebpage(uri, targetPath, cancellationToken);
            if (!File.Exists(targetPath)) return;

            if (File.Exists(cachedFilePath))
            {
                ShowNotificationIfWebsitesDiffer(targetPath, cachedFilePath, "Website has changed", uri);
                File.Delete(cachedFilePath);
            }
            
            File.Move(targetPath, cachedFilePath);
        }

        private async Task DownloadWebpage(Uri uri, string targetPath, CancellationToken cancellationToken)
        {
            await WebsiteDownloader.TryDownloadWebsiteWithPolicyAsync(uri, targetPath, cancellationToken);
        }

        private void ShowNotificationIfWebsitesDiffer(string targetPath, string cachedFilePath, string message, Uri uri)
        {
            var areEqual = FileContentComparer.Equals(targetPath, cachedFilePath);
            if (areEqual) return;

            Notifier.Notify(message, uri);
        }
    }
}