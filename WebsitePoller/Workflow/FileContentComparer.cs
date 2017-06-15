using System.IO;

namespace WebsitePoller.Workflow
{
    public sealed class FileContentComparer : IFileContentComparer
    {
        public bool Equals(string firstPath, string secondPath)
        {
            var first = new FileInfo(firstPath);
            var second = new FileInfo(secondPath);

            return Equals(first, second);
        }

        public bool Equals(FileInfo first, FileInfo second)
        {
            return CompareLength(first, second) 
                   && CompareBytewise(first, second);
        }

        public int GetHashCode(FileInfo obj)
        {
            return obj.GetHashCode();
        }

        private static bool CompareLength(FileInfo first, FileInfo second)
        {
            return first.Length == second.Length;
        }

        private static bool CompareBytewise(FileInfo first, FileInfo second)
        {
            using (var firstFileStream = first.OpenRead())
            using (var secondFileStream = second.OpenRead())
            {
                for (var i = 0; i < first.Length; i++)
                {
                    if (firstFileStream.ReadByte() != secondFileStream.ReadByte())
                        return false;
                }
            }
            return true;
        }
    }
}