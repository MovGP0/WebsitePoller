using Polly;

namespace WebsitePoller
{
    public interface IPolicyFactory
    {
        Policy CreateDownloadWebsitePolicy();
    }
}