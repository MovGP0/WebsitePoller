using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebsitePoller.Entities
{
    [Serializable]
    public sealed class PostalAddress : IEquatable<PostalAddress>, ISerializable
    {
        private readonly Version _version = new Version(1, 0);

        public string Title { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public DateTime BirthDate { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string StairsNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool IsTenant { get; set; }

        public PostalAddress()
        {
        }

        #region IEquatable
        public static IEqualityComparer<PostalAddress> PostalAddressComparer => new PostalAddressEqualityComparer();

        public bool Equals(PostalAddress other)
        {
            return PostalAddressComparer.Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return true;

            if (obj is PostalAddress pa)
            {
                return Equals(pa);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return PostalAddressComparer.GetHashCode(this);
        }
        #endregion

        #region ISerializable
        public PostalAddress(SerializationInfo info, StreamingContext context)
        {
            var version = info.GetValue<Version>("version");
            if (version.Major == 1)
            {
                Title = info.GetValue<string>("title");
                Salutation = info.GetValue<string>("salutation");
                FirstName = info.GetValue<string>("firstName");
                FamilyName = info.GetValue<string>("familyName");
                BirthDate = info.GetValue<DateTime>("birthDate");
                PostalCode = info.GetValue<int>("postalCode");
                City = info.GetValue<string>("city");
                Street = info.GetValue<string>("street");
                HouseNumber = info.GetValue<string>("houseNumber");
                StairsNumber = info.GetValue<string>("stairsNumber");
                ApartmentNumber = info.GetValue<string>("apartmentNumber");
                PhoneNumber = info.GetValue<string>("phoneNumber");
                EmailAddress = info.GetValue<string>("emailAddress");
                IsTenant = info.GetValue<bool>("isTenant");
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.FullTypeName = "PostalAddress";
            info.AssemblyName = "WebsitePoller";

            info.AddValue("version", _version);
            info.AddValue("title", Title);
            info.AddValue("salutation", Salutation);
            info.AddValue("firstName", FirstName);
            info.AddValue("familyName", FamilyName);
            info.AddValue("pirthDate", BirthDate);
            info.AddValue("postalCode", PostalCode);
            info.AddValue("city", City);
            info.AddValue("street", Street);
            info.AddValue("houseNumber", HouseNumber);
            info.AddValue("stairsNumber", StairsNumber);
            info.AddValue("apartmentNumber", ApartmentNumber);
            info.AddValue("phoneNumber", PhoneNumber);
            info.AddValue("emailAddress", EmailAddress);
            info.AddValue("isTenant", IsTenant);
        }
        #endregion
    }
}