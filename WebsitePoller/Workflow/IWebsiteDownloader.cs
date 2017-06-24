using System;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace WebsitePoller.Workflow
{
    public interface IWebsiteDownloader
    {
        Task<HtmlDocument> GetWebsiteOrNullWithPolicyAndLoggingAsync([NotNull]Uri url, [NotNull]string targetPath, CancellationToken cancellationToken);
        Task<HtmlDocument> GetWebsiteOrNullWithPolicyAsync([NotNull]Uri url, CancellationToken cancellationToken);
        Task<HtmlDocument> GetWebsiteOrNullAsync([NotNull]Uri url, CancellationToken cancellationToken);
    }
}