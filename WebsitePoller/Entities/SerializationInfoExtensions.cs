using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace WebsitePoller.Entities
{
    public static class SerializationInfoExtensions
    {
        public static T GetValue<T>([NotNull]this SerializationInfo info, [NotNull]string name)
        {
            return (T) info.GetValue(name, typeof(T));
        }

        public static void AddVersion([NotNull] this SerializationInfo info, Version version)
        {
            var ver = $"{version.Major}.{version.Minor}";
            info.AddValue("version", ver);
        }
    }
}