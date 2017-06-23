using JetBrains.Annotations;
using Polly;

namespace WebsitePoller
{
    public interface IPolicyFactory
    {
        [NotNull] Policy CreateDownloadWebsitePolicy();
        [NotNull] Policy CreatePostFormPolicy();
    }
}