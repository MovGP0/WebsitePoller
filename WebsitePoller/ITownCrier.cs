using System;

namespace WebsitePoller
{
    public interface ITownCrier
    {
        void Start();
        void Stop();
        event EventHandler Pray;
    }
}