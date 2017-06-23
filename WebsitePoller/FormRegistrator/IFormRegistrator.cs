using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace WebsitePoller.FormRegistrator
{
    public interface IFormRegistrator
    {
        [NotNull] Task PostRegistrationAsync([NotNull]Uri domain, [NotNull]string href, CancellationToken cancellationToken);
        [NotNull] Task PostRegistrationWithPolicyAsync([NotNull]Uri domain, [NotNull]string href, CancellationToken cancellationToken);
        [NotNull] Task PostRegistrationWithPolicyAndLoggingAsync([NotNull]Uri domain, [NotNull]string href, CancellationToken cancellationToken);
    }
}