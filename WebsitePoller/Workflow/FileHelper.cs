using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace WebsitePoller.Workflow
{
    public static class FileHelper
    {
        public static async Task CreateFileIfNotExistsAndAppendLinesToFileAsync([NotNull] string path, [NotNull] IEnumerable<string> lines)
        {
            CreateFileIfNotExists(path);
            await AppendLinesToFileAsync(path, lines);
        }

        public static void CreateFileIfNotExists([NotNull] string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentOutOfRangeException(nameof(path), path, "Was null or whitepsace.");
            if (!File.Exists(path)) File.Create(path);
        }

        public static async Task AppendLinesToFileAsync([NotNull] string path, [NotNull] IEnumerable<string> lines)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentOutOfRangeException(nameof(path), path, "Was null or whitepsace.");
            if (lines == null) throw new ArgumentNullException(nameof(lines));
            if (!File.Exists(path)) throw new FileNotFoundException("File not found.", nameof(path));

            using (var file = File.Open(path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(file))
            {
                foreach (var line in lines)
                {
                    await writer.WriteLineAsync(line);
                }
                await writer.FlushAsync();
            }
        }
        
        [NotNull]
        public static IEnumerable<string> GetFileLines([NotNull]string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentOutOfRangeException(nameof(path), path, "Was null or whitepsace.");
            if (!File.Exists(path)) yield break;

            using (var file = File.OpenRead(path))
            using (var reader = new StreamReader(file))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}