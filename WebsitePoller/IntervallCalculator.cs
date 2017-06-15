using NodaTime;

namespace WebsitePoller
{
    public class IntervallCalculator : IIntervallCalculator
    {
        private IClock SystemClock { get; }

        public IntervallCalculator(IClock systemClock)
        {
            SystemClock = systemClock;
        }

        public Duration CalculateDurationTillIntervall()
        {
            var currentTime = GetCurrentTime("Europe/Vienna");
            var minTime = new LocalTime(21, 00);
            var maxTime = new LocalTime(23, 00);

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