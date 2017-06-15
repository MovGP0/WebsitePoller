using System;

namespace WebsitePoller.Workflow
{
    public interface INotifier
    {
        void Notify(string message, Uri url);
    }
}