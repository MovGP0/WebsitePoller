using System;
using System.Text.RegularExpressions;
using AutoMapper;
using JetBrains.Annotations;
using NodaTime;

namespace WebsitePoller.Mappings
{
    public sealed class LocalTimeConverter : ITypeConverter<string, LocalTime>
    {
        private static LocalTime ConvertToLocalTime([NotNull]string value)
        {
            var pattern = new Regex("^(?<hours>[0-9]{1,2})(\\:(?<minutes>[0-9]{1,2}))?(\\:(?<seconds>[0-9]{1,2}))?$", RegexOptions.Compiled | RegexOptions.Singleline);

            if (!pattern.IsMatch(value))
            {
                throw new FormatException("Must be in format hh:mm:ss");
            }

            var groups = pattern.Match(value).Groups;
            int.TryParse(groups["hours"].Value, out int hours);
            int.TryParse(groups["minutes"].Value, out int minutes);
            int.TryParse(groups["seconds"].Value, out int seconds);

            if (hours < 0 || hours > 23) throw new ArgumentOutOfRangeException(nameof(hours), hours, "Must be between 0 and 23");
            if (minutes < 0 || minutes > 59) throw new ArgumentOutOfRangeException(nameof(minutes), minutes, "Must be between 0 and 59");
            if (seconds < 0 || seconds > 59) throw new ArgumentOutOfRangeException(nameof(seconds), seconds, "Must be between 0 and 59");

            return new LocalTime(hours, minutes, seconds);
        }

        public LocalTime Convert([NotNull]string source, LocalTime destination, ResolutionContext context)
        {
            if(string.IsNullOrWhiteSpace(source))
                throw new ArgumentOutOfRangeException(nameof(source), source, "May not be null or white space.");

            return ConvertToLocalTime(source);
        }
    }
}