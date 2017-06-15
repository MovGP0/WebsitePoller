using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebsitePoller.Workflow
{
    public interface IWebsiteDownloader
    {
        Task TryDownloadWebsiteWithPolicyAsync(Uri url, string targetPath, CancellationToken cancellationToken);
    }
}