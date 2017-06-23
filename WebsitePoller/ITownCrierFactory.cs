using JetBrains.Annotations;

namespace WebsitePoller
{
    public interface ITownCrierFactory
    {
        [NotNull] ITownCrier Invoke();
    }
}