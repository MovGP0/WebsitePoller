using Windows.UI.Notifications;

namespace WebsitePoller.Workflow
{
    public interface IToastNotifier
    {
        void Show(ToastNotification notification);
    }
}