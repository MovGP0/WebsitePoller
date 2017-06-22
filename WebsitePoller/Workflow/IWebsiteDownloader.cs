using System;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebsitePoller.Workflow
{
    public interface IWebsiteDownloader
    {
        Task<HtmlDocument> GetWebsiteOrNullWithPolicyAndLoggingAsync(Uri url, string targetPath, CancellationToken cancellationToken);
        Task<HtmlDocument> GetWebsiteOrNullWithPolicyAsync(Uri url, CancellationToken cancellationToken);
        Task<HtmlDocument> GetWebsiteOrNullAsync(Uri url, CancellationToken cancellationToken);
    }
}