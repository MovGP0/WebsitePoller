using Windows.UI.Notifications;

namespace WebsitePoller.Workflow
{
    public sealed class ToastNotifierWrapper : IToastNotifier
    {
        public ToastNotifierWrapper(ToastNotifier toastNotifier)
        {
            ToastNotifier = toastNotifier;
        }

        public ToastNotifier ToastNotifier { get; }

        public void Show(ToastNotification notification)
        {
            ToastNotifier.Show(notification);
        }
    }
}