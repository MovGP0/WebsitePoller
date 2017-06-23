using NodaTime;
using WebsitePoller.Setting;

namespace WebsitePoller
{
    public class IntervallCalculator : IIntervallCalculator
    {
        private IClock SystemClock { get; }
        private SettingsManager SettingsManager { get; }

        public IntervallCalculator(IClock systemClock, SettingsManager settingsManager)
        {
            SystemClock = systemClock;
            SettingsManager = settingsManager;
        }

        public Duration CalculateDurationTillIntervall()
        {
            var settings = SettingsManager.Settings;
            var currentTime = GetCurrentTime(settings.TimeZone);
            var minTime = settings.From;
            var maxTime = settings.Till;

            if (currentTime > minTime && currentTime < maxTime)
            {
                return Duration.Zero;
            }

            var period1 = minTime - currentTime;
            var period2 = currentTime - maxTime;

            var duration1 = period1.ToDuration();
            var duration2 = period2.ToDuration();

            return duration1.Milliseconds > 0d ? duration1 : duration2;
        }

        private LocalTime GetCurrentTime(string timeZone)
        {
            var now = SystemClock.GetCurrentInstant();
            var austria = DateTimeZoneProviders.Tzdb[timeZone];
            var localInstant = now.InZone(austria);
            return localInstant.TimeOfDay;
        }
    }
}