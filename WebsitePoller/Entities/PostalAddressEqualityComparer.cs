using System.Collections.Generic;

namespace WebsitePoller.Entities
{
    public sealed class PostalAddressEqualityComparer : IEqualityComparer<PostalAddress>
    {
        public bool Equals(PostalAddress x, PostalAddress y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals(x.Title, y.Title) 
                && string.Equals(x.Salutation, y.Salutation) 
                && string.Equals(x.FirstName, y.FirstName) 
                && string.Equals(x.FamilyName, y.FamilyName) 
                && x.BirthDate.Equals(y.BirthDate) 
                && x.PostalCode == y.PostalCode 
                && string.Equals(x.City, y.City) 
                && string.Equals(x.Street, y.Street) 
                && string.Equals(x.HouseNumber, y.HouseNumber) 
                && string.Equals(x.StairsNumber, y.StairsNumber) 
                && string.Equals(x.ApartmentNumber, y.ApartmentNumber) 
                && string.Equals(x.PhoneNumber, y.PhoneNumber) 
                && string.Equals(x.EmailAddress, y.EmailAddress) 
                && x.IsTenant == y.IsTenant;
        }

        public int GetHashCode(PostalAddress obj)
        {
            unchecked
            {
                var hashCode = (obj.Title != null ? obj.Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Salutation != null ? obj.Salutation.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.FirstName != null ? obj.FirstName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.FamilyName != null ? obj.FamilyName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ obj.BirthDate.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.PostalCode;
                hashCode = (hashCode * 397) ^ (obj.City != null ? obj.City.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Street != null ? obj.Street.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.HouseNumber != null ? obj.HouseNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.StairsNumber != null ? obj.StairsNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.ApartmentNumber != null ? obj.ApartmentNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.PhoneNumber != null ? obj.PhoneNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.EmailAddress != null ? obj.EmailAddress.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ obj.IsTenant.GetHashCode();
                return hashCode;
            }
        }
    }
}