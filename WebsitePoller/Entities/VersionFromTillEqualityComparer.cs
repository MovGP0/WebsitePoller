using System.Collections.Generic;

namespace WebsitePoller.Entities
{
    public sealed class VersionFromTillEqualityComparer : IEqualityComparer<SettingsStrings>
    {
        public bool Equals(SettingsStrings x, SettingsStrings y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return Equals(x.Version, y.Version) && string.Equals(x.From, y.From) && string.Equals(x.Till, y.Till);
        }

        public int GetHashCode(SettingsStrings obj)
        {
            unchecked
            {
                var hashCode = (obj.Version != null ? obj.Version.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.From != null ? obj.From.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Till != null ? obj.Till.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}