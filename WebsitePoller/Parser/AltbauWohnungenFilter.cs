using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Serilog;
using WebsitePoller.Entities;
using WebsitePoller.Setting;

namespace WebsitePoller.Parser
{
    public sealed class AltbauWohnungenFilter : IAltbauWohnungenFilter
    {
        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext<AltbauWohnungenFilter>();

        [NotNull]
        private SettingsManager SettingsManager { get; }

        public AltbauWohnungenFilter([NotNull]SettingsManager settingsManager)
        {
            SettingsManager = settingsManager ?? throw new ArgumentNullException(nameof(settingsManager));
        }

        [NotNull]
        public IEnumerable<AltbauWohnungInfo> Filter([NotNull]IEnumerable<AltbauWohnungInfo> altbauWohnungInfos)
        {
            var settings = SettingsManager.Settings;
            if(settings == null)
                throw new InvalidOperationException($"{nameof(SettingsManager.Settings)} not set.");

            return FilterCandidates(altbauWohnungInfos, settings);
        }

        [NotNull]
        private static IEnumerable<AltbauWohnungInfo> FilterCandidates([NotNull]IEnumerable<AltbauWohnungInfo> candidates, [NotNull]SettingsBase settings)
        {
            return candidates
                .Where(c => c.Eigenmittel <= settings.MaxEigenmittel)
                .Where(c => c.MonatlicheKosten <= settings.MaxMonatlicheKosten)
                .Where(c => c.NumberOfRooms >= settings.MinNumberOfRooms)
                .Where(c => settings.Cities.Contains(c.City))
                .Where(c => settings.PostalCodes.Contains(c.PostalCode));
        }
    }
}