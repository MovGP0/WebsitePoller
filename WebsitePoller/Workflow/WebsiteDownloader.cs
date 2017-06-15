using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace WebsitePoller.Workflow
{
    public sealed class WebsiteDownloader : IWebsiteDownloader
    {
        public WebsiteDownloader([NotNull]IPolicyFactory policyFactory)
        {
            PolicyFactory = policyFactory;
        }

        [NotNull]
        private IPolicyFactory PolicyFactory { get; }

        public async Task TryDownloadWebsiteWithPolicyAsync(Uri url, string targetPath, CancellationToken cancellationToken)
        {
            var policy = PolicyFactory.CreateDownloadWebsitePolicy();
            await policy.ExecuteAsync(token => DownloadWebsiteAsync(url, targetPath, token), cancellationToken);
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