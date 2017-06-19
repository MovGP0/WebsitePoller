using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Hjson;
using NodaTime;
using Serilog;
using WebsitePoller.Workflow;

namespace WebsitePoller
{
    public interface ISettingsLoader
    {
        void UpdateSettings();
    }

    public sealed class SettingsLoader : ISettingsLoader
    {
        private Settings Settings { get; }

        public SettingsLoader(Settings settings)
        {
            Settings = settings;
        }

        public void UpdateSettings()
        {
            var path = Path.Combine(Assembly.GetExecutingAssembly().GetDirectoryPath(), "settings.hson");
            if (!File.Exists(path)) return;

            var settings = Load(path);
            Settings.From = settings.From;
            Settings.Till = settings.Till;
            Settings.TimeZone = settings.TimeZone;
            Settings.Url = settings.Url;
        }
        
        private static ILogger Log => Serilog.Log.ForContext<SettingsLoader>();

        public Settings Load(string path)
        {
            var jsonObject = HjsonValue.Load(path).Qo();
            return ParseSettings(jsonObject);
        }

        private static Settings ParseSettings(JsonObject jsonObject)
        {
            var settings = new Settings();
            settings.Url = ParseUrl(jsonObject, "url", settings.Url);
            settings.From = ParseLocalTime(jsonObject, "from", settings.From);
            settings.Till = ParseLocalTime(jsonObject, "till", settings.Till);
            settings.TimeZone = ParseString(jsonObject, "timezone", settings.TimeZone);
            return settings;
        }

        private static string ParseString(JsonObject jsonObject, string valueName, string @default)
        {
            if (jsonObject.TryGetValue(valueName, out JsonValue timezone))
            {
                return timezone.ToString(Stringify.Plain);
            }

            Log.Information($"Setting '{valueName}' was not found in settings file.");
            return @default;
        }

        private static LocalTime ParseLocalTime(JsonObject jsonObject, string valueName, LocalTime @default)
        {
            if (jsonObject.TryGetValue(valueName, out JsonValue jsonValue))
            {
                if (TryConvertToLocalTime(jsonValue, out LocalTime localTime))
                {
                    return localTime;
                }

                Log.Error($"Could not parse value of '{valueName}' from settings file.");
                return @default;
            }

            Log.Information($"Setting '{valueName}' was not found in settings file.");
            return @default;
        }

        private static Uri ParseUrl(JsonObject jsonObject, string valueName, Uri @default)
        {
            if(jsonObject == null) throw new ArgumentNullException(nameof(jsonObject));
            if(string.IsNullOrWhiteSpace(valueName)) throw new ArgumentException("Must not be empty.", nameof(valueName));

            if (jsonObject.TryGetValue(valueName, out JsonValue jsonValue))
            {
                if (TryConvertToUri(jsonValue, out Uri uri))
                {
                    return uri;
                }

                Log.Error($"Could not parse value of '{valueName}' from settings file.");
                return @default;
            }

            Log.Information($"Setting '{valueName}' was not found in settings file.");
            return @default;
        }

        private static bool TryConvertToUri(JsonValue value, out Uri uri)
        {
            try
            {
                var urlString = value.ToString(Stringify.Plain);
                if (string.IsNullOrWhiteSpace(urlString))
                {
                    uri = new Uri("");
                    return false;
                }

                uri = new Uri(urlString);
                return true;
            }
            catch
            {
                uri = new Uri("");
                return false;
            }
        }

        private static bool TryConvertToLocalTime(JsonValue value, out LocalTime localTime)
        {
            try
            {
                var pattern = new Regex("(?<hours>[0-9]{1,2})(\\:(?<minutes>[0-9]{1,2}))?(\\:(?<seconds>[0-9]{1,2}))?");

                var valueString = value.ToString(Stringify.Plain);

                if (!pattern.IsMatch(valueString))
                {
                    throw new FormatException("Must be in format hh:mm:ss");
                }

                var groups = pattern.Match(valueString).Groups;
                int.TryParse(groups["hours"].Value, out int hours);
                int.TryParse(groups["minutes"].Value, out int minutes);
                int.TryParse(groups["seconds"].Value, out int seconds);

                if (hours < 0 || hours > 23) throw new ArgumentOutOfRangeException(nameof(hours), hours, "Must be between 0 and 23");
                if (minutes < 0 || minutes > 59) throw new ArgumentOutOfRangeException(nameof(minutes), minutes, "Must be between 0 and 59");
                if (seconds < 0 || seconds > 59) throw new ArgumentOutOfRangeException(nameof(seconds), seconds, "Must be between 0 and 59");

                localTime = new LocalTime(hours, minutes, seconds);
                return true;
            }
            catch
            {
                localTime = LocalTime.MinValue;
                return false;
            }
        }
    }
}