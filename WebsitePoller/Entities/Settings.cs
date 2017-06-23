using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NodaTime;

namespace WebsitePoller.Entities
{
    public sealed class Settings : SettingsBase, IEquatable<Settings>
    {
        public LocalTime From { get; set; }
        public LocalTime Till { get; set; }
        
        #region IEquatable
        [NotNull]
        private static IEqualityComparer<Settings> SettingsComparer => new SettingsEqualityComparer();

        public bool Equals(Settings other)
        {
            return SettingsBaseComparer.Equals(this, other)
                   && SettingsComparer.Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (obj is Settings settings)
            {
                return Equals(settings);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (SettingsBaseComparer.GetHashCode(this) * 397) 
                    ^ SettingsComparer.GetHashCode(this);
            }
        }
        
        public static bool operator ==(Settings settings, Settings other)
        {
            if (ReferenceEquals(settings, other)) return true;
            if (ReferenceEquals(settings, null)) return false;
            if (ReferenceEquals(other, null)) return false;

            return settings.Equals(other);
        }

        public static bool operator !=(Settings settings, Settings other)
        {
            return !(settings == other);
        }
        #endregion

    }
}
