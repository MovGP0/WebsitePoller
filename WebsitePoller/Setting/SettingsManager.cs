using System;
using WebsitePoller.Entities;

namespace WebsitePoller.Setting
{
    /// <threadsafety static="true" instance="true"/>
    public sealed class SettingsManager
    {
        private Settings _settings;
        private readonly object _settingsLock = new object();

        public Settings Settings
        {
            get
            {
                lock (_settingsLock)
                {
                    return _settings;
                }
            }
            set
            {
                if(value == null) throw new ArgumentNullException(nameof(value));

                lock (_settingsLock)
                {
                    _settings = value;
                }
            }
        }
    }
}