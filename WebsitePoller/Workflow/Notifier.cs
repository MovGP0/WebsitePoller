using System;
using Windows.UI.Notifications;
using Serilog;

namespace WebsitePoller.Workflow
{
    public sealed class Notifier : INotifier
    {
        public Notifier(IToastNotifier toastNotifier)
        {
            ToastNotifier = toastNotifier;
        }

        private static ILogger Log => Serilog.Log.ForContext<Notifier>();
        private IToastNotifier ToastNotifier { get; }

        public void Notify(string message, Uri url)
        {
            var toastXml = NotificationMessageFactory.GetToastXml(message, url);
            var toast = new ToastNotification(toastXml);

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