using NodaTime;

namespace WebsitePoller
{
    public class IntervallCalculator : IIntervallCalculator
    {
        private IClock SystemClock { get; }
        private Settings Settings { get; }

        public IntervallCalculator(IClock systemClock, Settings settings)
        {
            SystemClock = systemClock;
            Settings = settings;
        }

        public Duration CalculateDurationTillIntervall()
        {
            var currentTime = GetCurrentTime(Settings.TimeZone);
            var minTime = Settings.From;
            var maxTime = Settings.Till;

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