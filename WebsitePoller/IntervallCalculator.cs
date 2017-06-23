using System;
using JetBrains.Annotations;
using NodaTime;
using WebsitePoller.Setting;

namespace WebsitePoller
{
    public class IntervallCalculator : IIntervallCalculator
    {
        [NotNull]
        private IClock SystemClock { get; }

        [NotNull]
        private SettingsManager SettingsManager { get; }

        public IntervallCalculator([NotNull]IClock systemClock, [NotNull]SettingsManager settingsManager)
        {
            SystemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
            SettingsManager = settingsManager ?? throw new ArgumentNullException(nameof(settingsManager));
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

            return duration1.TotalMilliseconds > 0d ? duration1 : duration2;
        }

        private LocalTime GetCurrentTime([NotNull]string timeZone)
        {
            if (timeZone == null) throw new ArgumentException("May not be null.", nameof(timeZone));
            if (string.IsNullOrWhiteSpace(timeZone)) throw new ArgumentException("May not be empty.", nameof(timeZone));

            var now = SystemClock.GetCurrentInstant();
            var dateTimeZone = DateTimeZoneProviders.Tzdb[timeZone];

            if(dateTimeZone == null) throw new ArgumentException($"Time zone '{timeZone}' could not be found.");

            var localInstant = now.InZone(dateTimeZone);
            return localInstant.TimeOfDay;
        }
    }
}