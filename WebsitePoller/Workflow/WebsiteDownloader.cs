using System;
using System.Net;
using System.Text;
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
        private static ILogger Log => Serilog.Log.ForContext<WebsiteDownloader>();

        public WebsiteDownloader([NotNull]IPolicyFactory policyFactory)
        {
            PolicyFactory = policyFactory;
        }

        [NotNull]
        private IPolicyFactory PolicyFactory { get; }

        public async Task<HtmlDocument> GetWebsiteOrNullWithPolicyAsync(Uri url, string targetPath, CancellationToken cancellationToken)
        {
            var policy = PolicyFactory.CreateDownloadWebsitePolicy();
            try
            {
                Log.Verbose($"Downloading {url}");
                var document = await policy.ExecuteAsync(token => GetWebsiteOrNullAsync(url, token), cancellationToken);
                return document;
            }
            catch(Exception e)
            {
                Log.Error(e, e.Message);
                return null;
            }
        }
        
        private static async Task<HtmlDocument> GetWebsiteOrNullAsync(Uri url, CancellationToken cancellationToken)
        {
            var resource = url.PathAndQuery;
            var domain = url.GetDomain();
            var client = new RestClient(domain) { Encoding = Encoding.UTF8 };

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

    public static class UriExtensions
    {
        public static Uri GetDomain(this Uri url)
        {
            return new Uri($"{url.Scheme}://{url.DnsSafeHost}:{url.Port}", UriKind.Absolute);
        }
    }
}