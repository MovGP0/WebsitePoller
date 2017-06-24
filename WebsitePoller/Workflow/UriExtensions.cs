using System;
using JetBrains.Annotations;

namespace WebsitePoller.Workflow
{
    public static class UriExtensions
    {
        [NotNull]
        public static Uri GetDomain([NotNull]this Uri url)
        {
            if(url == null) throw new ArgumentNullException(nameof(url));

            return new Uri($"{url.Scheme}://{url.DnsSafeHost}:{url.Port}", UriKind.Absolute);
        }
    }
}