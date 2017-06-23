using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace WebsitePoller.Entities
{
    [Serializable]
    public sealed class SettingsStrings : SettingsBase, ISerializable, IEquatable<SettingsStrings>
    {
        internal readonly Version Version = new Version(1, 0);

        public string From { get; set; }
        public string Till { get; set; }
        
        public SettingsStrings()
        {
        }

        #region ISerializable
        public SettingsStrings([NotNull]SerializationInfo info, StreamingContext context) : base(info, context)
        {
            var version = info.GetValue<Version>("version");
            if (version.Major == 1)
            {
                From = info.GetValue<string>("from");
                Till = info.GetValue<string>("till");
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            GetObjectDataBase(info, context);

            info.FullTypeName = "SettingsStrings";
            info.AssemblyName = "WebsitePoller";

            info.AddVersion(Version);
            info.AddValue("from", From);
            info.AddValue("till", Till);
        }
        #endregion

        #region IEquatable
        public static IEqualityComparer<SettingsStrings> VersionFromTillComparer => new VersionFromTillEqualityComparer();
        public bool Equals(SettingsStrings other)
        {
            return SettingsBaseComparer.Equals(this, other)
                && VersionFromTillComparer.Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is SettingsStrings ss) return Equals(ss);
            return false;
        }

        public override int GetHashCode()
        {
            return (VersionFromTillComparer.GetHashCode(this) * 397)
                ^ SettingsBaseComparer.GetHashCode(this);
        }

        public static bool operator ==(SettingsStrings settings, SettingsStrings other)
        {
            if (ReferenceEquals(settings, other)) return true;
            if (ReferenceEquals(settings, null)) return false;
            if (ReferenceEquals(other, null)) return false;

            return settings.Equals(other);
        }

        public static bool operator !=(SettingsStrings settings, SettingsStrings other)
        {
            return !(settings == other);
        }
        #endregion
    }
}