using System.Collections.Generic;

namespace WebsitePoller.Entities
{
    public sealed class AltbauWohnungInfoEqualityComparer : IEqualityComparer<AltbauWohnungInfo>
    {
        public bool Equals(AltbauWohnungInfo x, AltbauWohnungInfo y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals(x.City, y.City) && x.Eigenmittel == y.Eigenmittel && string.Equals(x.Href, y.Href) && x.MonatlicheKosten == y.MonatlicheKosten && x.NumberOfRooms == y.NumberOfRooms && x.PostalCode == y.PostalCode && string.Equals(x.Street, y.Street);
        }

        public int GetHashCode(AltbauWohnungInfo obj)
        {
            unchecked
            {
                var hashCode = (obj.City != null ? obj.City.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ obj.Eigenmittel.GetHashCode();
                hashCode = (hashCode * 397) ^ (obj.Href != null ? obj.Href.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ obj.MonatlicheKosten.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.NumberOfRooms;
                hashCode = (hashCode * 397) ^ obj.PostalCode;
                hashCode = (hashCode * 397) ^ (obj.Street != null ? obj.Street.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}