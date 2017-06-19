using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Serilog;

namespace WebsitePoller.Workflow
{
    public sealed class WebsiteDownloader : IWebsiteDownloader
    {
        private static ILogger Log => Serilog.Log.ForContext<WebsiteDownloader>();

        public WebsiteDownloader([NotNull]IPolicyFactory policyFactory)
        {
            PolicyFactory = policyFactory;
        }

        [NotNull]
        private IPolicyFactory PolicyFactory { get; }

        public async Task TryDownloadWebsiteWithPolicyAsync(Uri url, string targetPath, CancellationToken cancellationToken)
        {
            var policy = PolicyFactory.CreateDownloadWebsitePolicy();
            try
            {
                await policy.ExecuteAsync(token => DownloadWebsiteAsync(url, targetPath, token), cancellationToken);
            }
            catch(Exception e)
            {
                Log.Error(e, e.Message);
            }
        }
        
        private static async Task DownloadWebsiteAsync(Uri url, string targetPath, CancellationToken cancellationToken)
        {
            using (var client = new WebClient())
            {
                cancellationToken.Register(() => client.CancelAsync());
                await client.DownloadFileTaskAsync(url, targetPath);
            }
        }
    }
}