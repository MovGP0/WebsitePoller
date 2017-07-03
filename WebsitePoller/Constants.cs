using System;
using JetBrains.Annotations;

namespace WebsitePoller
{
    public static class Constants
    {
        public const string ServiceName = "WebsitePoller";
        public const string LogName = "Application";

        [NotNull]
        public static readonly string MachineName = Environment.MachineName;
    }
}