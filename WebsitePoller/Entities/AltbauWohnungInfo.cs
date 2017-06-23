using System;
using System.Runtime.Serialization;

namespace WebsitePoller.Entities
{
    [Serializable]
    public sealed class AltbauWohnungInfo : IEquatable<AltbauWohnungInfo>, ISerializable
    {
        private readonly Version _version = new Version(1,0);
        
        public string Href { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal Eigenmittel { get; set; }
        public decimal MonatlicheKosten { get; set; }

        public AltbauWohnungInfo()
        {
        }

        #region IEquatable
        private static readonly AltbauWohnungInfoEqualityComparer EqualityComparer
            = new AltbauWohnungInfoEqualityComparer();


        public bool Equals(AltbauWohnungInfo other)
        {
            return EqualityComparer.Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return true;

            if (obj is AltbauWohnungInfo awi)
            {
                return Equals(awi);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return EqualityComparer.GetHashCode(this);
        }

        #endregion

        #region ISerializable
        public AltbauWohnungInfo(SerializationInfo info, StreamingContext context)
        {
            var version = info.GetValue<Version>("version");
            if (version.Major == 1)
            {
                Href = info.GetValue<string>("href");
                PostalCode = info.GetValue<int>("postalCode");
                City = info.GetValue<string>("city");
                Street = info.GetValue<string>("street");
                NumberOfRooms = info.GetValue<int>("numberOfRooms");
                Eigenmittel = info.GetValue<decimal>("eigenmittel");
                MonatlicheKosten = info.GetValue<decimal>("monatlicheKosten");
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.FullTypeName = "AltbauWohnungInfo";
            info.AssemblyName = "WebsitePoller";

            info.AddValue("version", _version);
            info.AddValue("href", Href);
            info.AddValue("postalCode", PostalCode);
            info.AddValue("city", City);
            info.AddValue("street", Street);
            info.AddValue("numberOfRooms", NumberOfRooms);
            info.AddValue("eigenmittel", Eigenmittel);
            info.AddValue("monatlicheKosten", MonatlicheKosten);
        }
        #endregion
    }
}