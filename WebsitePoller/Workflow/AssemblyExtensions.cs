using System;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;

namespace WebsitePoller.Workflow
{
    public static class AssemblyExtensions
    {
        public static string GetDirectoryPath([NotNull]this Assembly assembly)
        {
            if(assembly == null) throw new ArgumentNullException(nameof(assembly));
            var filePath = new Uri(assembly.CodeBase).LocalPath;
            return Path.GetDirectoryName(filePath);
        }
    }
}