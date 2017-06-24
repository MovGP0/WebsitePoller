using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using RestSharp;
using Serilog;
using HtmlAgilityPack;

namespace WebsitePoller.Workflow
{
    public sealed class WebsiteDownloader : IWebsiteDownloader
    {
        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext<WebsiteDownloader>();

        [NotNull]
        private IPolicyFactory PolicyFactory { get; }

        [NotNull]
        private Func<Uri, IRestClient> RestClientFactory { get; }

        public WebsiteDownloader(
            [NotNull]IPolicyFactory policyFactory, 
            [NotNull]Func<Uri, IRestClient> restClientFactory)
        {
            PolicyFactory = policyFactory ?? throw new ArgumentNullException(nameof(policyFactory));
            RestClientFactory = restClientFactory ?? throw new ArgumentNullException(nameof(restClientFactory));
        }

        public async Task<HtmlDocument> GetWebsiteOrNullWithPolicyAndLoggingAsync(Uri url, string targetPath, CancellationToken cancellationToken)
        {
            if(url == null) throw new ArgumentNullException(nameof(url));
            if(targetPath == null) throw new ArgumentNullException(nameof(targetPath));

            try
            {
                Log.Verbose($"Downloading {url}");
                return await GetWebsiteOrNullWithPolicyAsync(url, cancellationToken);
            }
            catch(Exception e)
            {
                Log.Error(e, e.Message);
                return null;
            }
        }

        public async Task<HtmlDocument> GetWebsiteOrNullWithPolicyAsync(Uri url, CancellationToken cancellationToken)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            
            var policy = PolicyFactory.CreateDownloadWebsitePolicy();
            return await policy.ExecuteAsync(token => GetWebsiteOrNullAsync(url, token), cancellationToken);
        }

        public async Task<HtmlDocument> GetWebsiteOrNullAsync(Uri url, CancellationToken cancellationToken)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            var resource = url.PathAndQuery;
            var domain = url.GetDomain();
            var client = RestClientFactory(domain);

            var request = new RestRequest(resource, Method.GET);
            var response = await client.ExecuteTaskAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Log.Warning($"Could not download ressource '{url}'. Status code {response.StatusCode}");
                return null;
            }

            var document = new HtmlDocument();
            document.LoadHtml(response.Content);
            return document;
        }
    }
}