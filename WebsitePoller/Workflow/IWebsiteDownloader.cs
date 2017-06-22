using System;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebsitePoller.Workflow
{
    public interface IWebsiteDownloader
    {
        Task<HtmlDocument> GetWebsiteOrNullWithPolicyAsync(Uri url, string targetPath, CancellationToken cancellationToken);
    }
}