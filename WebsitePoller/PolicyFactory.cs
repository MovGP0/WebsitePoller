using System;
using Polly;

namespace WebsitePoller
{
    public sealed class PolicyFactory : IPolicyFactory
    {
        public Policy CreateDownloadWebsitePolicy()
        {
            var retry = Policy
                .Handle<InvalidOperationException>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2d, retryAttempt)));

            var breaker = Policy.Handle<InvalidOperationException>()
                .CircuitBreakerAsync(2, TimeSpan.FromMinutes(5));

            var timeout = Policy.TimeoutAsync(TimeSpan.FromSeconds(30));
            var bulkhead = Policy.BulkheadAsync(1);

            return Policy.WrapAsync(retry, breaker, timeout, bulkhead);
        }
    }
}