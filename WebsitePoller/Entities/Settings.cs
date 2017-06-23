using NodaTime;

namespace WebsitePoller.Entities
{
    public sealed class Settings : SettingsBase
    {
        public LocalTime From { get; set; }
        public LocalTime Till { get; set; }
    }
}
