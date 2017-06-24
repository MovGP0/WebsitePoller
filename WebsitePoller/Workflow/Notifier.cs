using System;
using Windows.UI.Notifications;
using JetBrains.Annotations;
using Serilog;
using XmlDocument = Windows.Data.Xml.Dom.XmlDocument;

namespace WebsitePoller.Workflow
{
    public sealed class Notifier : INotifier
    {
        public Notifier(
            [NotNull] IToastNotifier toastNotifier, 
            [NotNull] Func<XmlDocument, ToastNotification> toastNotificationFactory)
        {
            ToastNotificationFactory = toastNotificationFactory ?? throw new ArgumentNullException(nameof(toastNotificationFactory));
            ToastNotifier = toastNotifier ?? throw new ArgumentNullException(nameof(toastNotifier));
        }

        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext<Notifier>();

        [NotNull]
        private IToastNotifier ToastNotifier { get; }

        [NotNull]
        private Func<XmlDocument, ToastNotification> ToastNotificationFactory { get; }

        public void Notify([NotNull]string message, [NotNull]Uri url)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (url == null) throw new ArgumentNullException(nameof(url));

            var toastXml = NotificationMessageFactory.GetToastXml(message, url);
            var toast = ToastNotificationFactory(toastXml);

            toast.Activated += ToastActivated;
            toast.Dismissed += ToastDismissed;
            toast.Failed += ToastFailed;

            ToastNotifier.Show(toast);
        }

        private static void ToastFailed(ToastNotification sender, ToastFailedEventArgs args)
        {
            Log.Warning($"Could not show toast message with error code {args.ErrorCode}.");
        }

        private static void ToastDismissed(ToastNotification sender, ToastDismissedEventArgs args)
        {
            Log.Debug($"Notification dismissed with reason {args.Reason}");
        }

        private static void ToastActivated(ToastNotification sender, object args)
        {
            Log.Debug("Notification activated.");
        }
    }
}