﻿using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace WebsitePoller.Workflow
{
    public static class AssemblyExtensions
    {
        public static string GetDirectoryPath(this Assembly assembly)
        {
            var filePath = new Uri(assembly.CodeBase).LocalPath;
            return Path.GetDirectoryName(filePath);
        }
    }

    public sealed class ExecuteWorkFlowCommand : IExecuteWorkFlowCommand
    {
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

            await DownloadWebpage(Settings.Url, targetPath, cancellationToken);
            if (!File.Exists(targetPath)) return;

            if (File.Exists(cachedFilePath))
            {
                ShowNotificationIfWebsitesDiffer(targetPath, cachedFilePath, "Website has changed", Settings.Url);
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