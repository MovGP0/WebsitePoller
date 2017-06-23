using System.Collections.Generic;
using System.Linq;

namespace WebsitePoller.Entities
{
    public sealed class SettingsBaseEqualityComparer : IEqualityComparer<SettingsBase>
    {
        public bool Equals(SettingsBase x, SettingsBase y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;

            return x.Cities.SequenceEqual(y.Cities) 
                && x.MaxEigenmittel == y.MaxEigenmittel 
                && x.MaxMonatlicheKosten == y.MaxMonatlicheKosten 
                && x.MinNumberOfRooms == y.MinNumberOfRooms 
                && x.PollingIntervallInSeconds == y.PollingIntervallInSeconds 
                && Equals(x.PostalAddress, y.PostalAddress) 
                && x.PostalCodes.SequenceEqual(y.PostalCodes) 
                && string.Equals(x.TimeZone, y.TimeZone)
                && x.Url == x.Url;
        }

        public int GetHashCode(SettingsBase obj)
        {
            unchecked
            {
                var hashCode = ArrayHashExtensions.GetHashCode(obj.Cities);
                hashCode = (hashCode * 397) ^ obj.MaxEigenmittel.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.MaxMonatlicheKosten.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.MinNumberOfRooms;
                hashCode = (hashCode * 397) ^ obj.PollingIntervallInSeconds;
                hashCode = (hashCode * 397) ^ (obj.PostalAddress != null ? obj.PostalAddress.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ArrayHashExtensions.GetHashCode(obj.PostalCodes);
                hashCode = (hashCode * 397) ^ (obj.TimeZone != null ? obj.TimeZone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Url != null ? obj.Url.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}