using System;
using NodaTime;

namespace WebsitePoller
{
    public sealed class Settings
    {
        public string TimeZone { get; set; } = "";
        public Uri Url { get; set; } = new Uri("http://m.sozialbau.at/wohnungen/altbau/");
        public LocalTime From { get; set; } = new LocalTime(21, 00);
        public LocalTime Till { get; set; } = new LocalTime(23, 00);
    }
}
