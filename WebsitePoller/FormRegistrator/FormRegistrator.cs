using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using RestSharp;
using Serilog;
using WebsitePoller.Setting;

namespace WebsitePoller.FormRegistrator
{
    public sealed class FormRegistrator : IFormRegistrator
    {
        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext<FormRegistrator>();

        [NotNull]
        private SettingsManager SettingsManager { get; }

        [NotNull]
        private Func<Uri, IRestClient> RestClientFactory { get; }

        [NotNull]
        private IPolicyFactory PolicyFactory { get; }

        public FormRegistrator(
            [NotNull] SettingsManager settingsManager, 
            [NotNull] Func<Uri, IRestClient> restClientFactory, 
            [NotNull] IPolicyFactory policyFactory)
        {
            SettingsManager = settingsManager;
            RestClientFactory = restClientFactory;
            PolicyFactory = policyFactory;
        }

        public async Task PostRegistrationWithPolicyAndLoggingAsync(Uri domain, string href, CancellationToken cancellationToken)
        {
            try
            {
                await PostRegistrationWithPolicyAsync(domain, href, cancellationToken);
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
            }
        }

        public async Task PostRegistrationAsync(Uri domain, string href, CancellationToken cancellationToken)
        {
            var postalAddress = SettingsManager.Settings.PostalAddress;
            var contactRequest = postalAddress.BuildContactRequest(href);
            var client = RestClientFactory(domain);

            var contactResponse = await client.ExecuteTaskAsync(contactRequest, cancellationToken);
            if (contactResponse.StatusCode != HttpStatusCode.OK)
            {
                Log.Error($"Coud not post form for {href}.");
            }
        }

        public async Task PostRegistrationWithPolicyAsync(Uri domain, string href, CancellationToken cancellationToken)
        {
            var policy = PolicyFactory.CreatePostFormPolicy();
            await policy.ExecuteAsync(async() => await PostRegistrationAsync(domain, href, cancellationToken));
        }
    }
}
