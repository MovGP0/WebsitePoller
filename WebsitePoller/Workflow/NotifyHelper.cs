using System;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using Serilog;
using WebsitePoller.Entities;

namespace WebsitePoller.Workflow
{
    public sealed class NotifyHelper : INotifyHelper
    {
        public NotifyHelper([NotNull] INotifier notifier)
        {
            Notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));

        }

        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext<NotifyHelper>();

        [NotNull]
        private INotifier Notifier { get; }
        
        public void ShowNotificationThatWebsiteHasChanged(string message, Uri uri)
        {
            Log.Verbose("Informing user that site has changed");
            Notifier.Notify(message, uri);
        }

        public void ShowNotificationThatRegisteredForOffer(AltbauWohnungInfo offer, Uri domain)
        {
            Log.Verbose("Informing user that offer was posted");

            var message = new StringBuilder()
                .AppendLine("Registered for this offer: ")
                .AppendLine(offer.Street)
                .Append(offer.PostalCode).Append(" ").AppendLine(offer.City)
                .Append("Own funds: ").AppendLine(offer.Eigenmittel.ToString(CultureInfo.CurrentCulture))
                .Append("Monthly cost: ").AppendLine(offer.MonatlicheKosten.ToString(CultureInfo.CurrentCulture))
                .Append("Number of rooms: ").AppendLine(offer.NumberOfRooms.ToString())
                .ToString();

            var uri = domain;
            try
            {
                uri = new Uri(domain, offer.Href);
            }
            catch(Exception e)
            {
                Log.Error(e, e.Message);
            }
            Notifier.Notify(message, uri);
        }
    }
}
