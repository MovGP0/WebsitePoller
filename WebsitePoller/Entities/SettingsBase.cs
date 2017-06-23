using System;
using System.Collections.Generic;

namespace WebsitePoller.Entities
{
    public abstract class SettingsBase
    {
        public string TimeZone { get; set; }
        public Uri Url { get; set; }
        public int[] PostalCodes { get; set; }
        public string[] Cities { get; set; }
        public decimal MaxEigenmittel { get; set; }
        public decimal MaxMonatlicheKosten { get; set; }
        public int MinNumberOfRooms { get; set; }
        public PostalAddress PostalAddress { get; set; }
        public int PollingIntervallInSeconds { get; set; }
        
        protected static IEqualityComparer<SettingsBase> SettingsBaseComparer { get; } = new SettingsBaseEqualityComparer();
    }
}