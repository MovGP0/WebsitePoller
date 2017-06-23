using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;

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

        protected SettingsBase()
        {
        }

        protected static IEqualityComparer<SettingsBase> SettingsBaseComparer => new SettingsBaseEqualityComparer();

        #region ISerializable
        protected SettingsBase([NotNull]SerializationInfo info, StreamingContext context)
        {
            TimeZone = info.GetValue<string>("timeZone");
            Url = info.GetValue<Uri>("url");
            MaxEigenmittel = info.GetValue<decimal>("maxEigenmittel");
            MaxMonatlicheKosten = info.GetValue<decimal>("maxMonatlicheKosten");
            MinNumberOfRooms = info.GetValue<int>("minNumberOfRooms");
            PostalAddress = info.GetValue<PostalAddress>("postalAddress");
            PollingIntervallInSeconds = info.GetValue<int>("pollingIntervallInSeconds");

            PostalCodes = info.GetValue<int[]>("postalCodes");
            Cities = info.GetValue<string[]>("cities");
        }

        protected void GetObjectDataBase([NotNull]SerializationInfo info, StreamingContext context)
        {
            info.AddValue("timeZone", TimeZone);
            info.AddValue("url", Url);
            info.AddValue("postalCodes", PostalCodes);
            info.AddValue("cities", Cities);
            info.AddValue("maxEigenmittel", MaxEigenmittel);
            info.AddValue("maxMonatlicheKosten", MaxMonatlicheKosten);
            info.AddValue("minNumberOfRooms", MinNumberOfRooms);
            info.AddValue("postalAddress", PostalAddress);
            info.AddValue("pollingIntervallInSeconds", PollingIntervallInSeconds);
        }
        #endregion
    }
}