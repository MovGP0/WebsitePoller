using System;

namespace WebsitePoller.Workflow
{
    public static class UriExtensions
    {
        public static Uri GetDomain(this Uri url)
        {
            return new Uri($"{url.Scheme}://{url.DnsSafeHost}:{url.Port}", UriKind.Absolute);
        }
    }
}