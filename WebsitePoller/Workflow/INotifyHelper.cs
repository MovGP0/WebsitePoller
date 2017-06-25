using System;
using WebsitePoller.Entities;

namespace WebsitePoller.Workflow
{
    public interface INotifyHelper
    {
        void ShowNotificationThatWebsiteHasChanged(string message, Uri uri);
        void ShowNotificationThatRegisteredForOffer(AltbauWohnungInfo offer);
    }
}