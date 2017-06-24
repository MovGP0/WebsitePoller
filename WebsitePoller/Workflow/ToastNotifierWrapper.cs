using System;
using Windows.UI.Notifications;
using JetBrains.Annotations;

namespace WebsitePoller.Workflow
{
    public sealed class ToastNotifierWrapper : IToastNotifier
    {
        public ToastNotifierWrapper([NotNull]ToastNotifier toastNotifier)
        {
            ToastNotifier = toastNotifier ?? throw new ArgumentNullException(nameof(toastNotifier));
        }

        [NotNull]
        public ToastNotifier ToastNotifier { get; }

        public void Show([NotNull]ToastNotification notification)
        {
            if(notification == null) throw new ArgumentNullException(nameof(notification));
            ToastNotifier.Show(notification);
        }
    }
}