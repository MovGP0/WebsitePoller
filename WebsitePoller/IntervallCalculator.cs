using System;
using JetBrains.Annotations;
using NodaTime;
using WebsitePoller.Entities;
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
            var localInstant = GetCurrentDateTime(settings.TimeZone);
            var minTime = settings.From;
            var maxTime = settings.Till;

            var currentTime = localInstant.TimeOfDay;
            if (minTime < maxTime && currentTime >= minTime && currentTime <= maxTime)
            {
                return Duration.Zero;
            }
            
            if (minTime > maxTime && (IsBetweenMinTimeAndMidnight(currentTime, minTime) || IsBetweenMidnightAndMaxTime(currentTime, maxTime)))
            {
                return Duration.Zero;
            }

            var period1 = minTime - currentTime;
            var duration1 = period1.ToDuration();

            if (duration1.TotalMilliseconds > 0d)
                return duration1;
            
            var nextMinInstant = GetTomorrowsMinInstant(localInstant, settings);
            return nextMinInstant - localInstant;
        }

        private static bool IsBetweenMidnightAndMaxTime(LocalTime currentTime, LocalTime maxTime)
        {
            return currentTime >= new LocalTime(00, 00) && currentTime <= maxTime;
        }

        private static bool IsBetweenMinTimeAndMidnight(LocalTime currentTime, LocalTime minTime)
        {
            return currentTime <= new LocalTime(23, 59, 59, 999) && currentTime >= minTime;
        }

        private static ZonedDateTime GetTomorrowsMinInstant(ZonedDateTime localInstant, Settings settings)
        {
            var minTime = settings.From;
            var tomorrowsMin = new LocalDateTime(localInstant.Year, localInstant.Month, localInstant.Day, minTime.Hour, minTime.Minute, minTime.Second).PlusDays(1);
            
            var dateTimeZone = DateTimeZoneProviders.Tzdb[settings.TimeZone];
            var zoned = dateTimeZone.AtLeniently(tomorrowsMin);
            
            return CorrectDaylightSavingTime(minTime, zoned);
        }

        private static ZonedDateTime CorrectDaylightSavingTime(LocalTime minTime, ZonedDateTime zoned)
        {
            var diffHour = minTime.Hour - zoned.Hour;
            var diffMin = minTime.Minute - zoned.Minute;
            var diffSec = minTime.Second - zoned.Second;

            return zoned
                .PlusHours(diffHour)
                .PlusMinutes(diffMin)
                .PlusSeconds(diffSec);
        }

        private ZonedDateTime GetCurrentDateTime([NotNull]string timeZone)
        {
            if (timeZone == null) throw new ArgumentException("May not be null.", nameof(timeZone));
            if (string.IsNullOrWhiteSpace(timeZone)) throw new ArgumentException("May not be empty.", nameof(timeZone));

            var now = SystemClock.GetCurrentInstant();
            var dateTimeZone = DateTimeZoneProviders.Tzdb[timeZone];

            if(dateTimeZone == null) throw new ArgumentException($"Time zone '{timeZone}' could not be found.");

            var localInstant = now.InZone(dateTimeZone);
            return localInstant;
        }
    }
}