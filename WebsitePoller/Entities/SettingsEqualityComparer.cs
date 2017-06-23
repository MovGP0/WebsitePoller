using System.Collections.Generic;

namespace WebsitePoller.Entities
{
    public sealed class SettingsEqualityComparer : IEqualityComparer<Settings>
    {
        public bool Equals(Settings x, Settings y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;

            return x.From.Equals(y.From) 
                && x.Till.Equals(y.Till);
        }

        public int GetHashCode(Settings obj)
        {
            unchecked
            {
                var hashCode = obj.From.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Till.GetHashCode();
                return hashCode;
            }
        }
    }
}