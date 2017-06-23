using NodaTime;

namespace WebsitePoller.Setting
{
    public sealed class Settings : SettingsBase
    {
        public LocalTime From { get; set; }
        public LocalTime Till { get; set; }
    }
}
