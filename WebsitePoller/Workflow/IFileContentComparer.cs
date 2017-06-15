using System.Collections.Generic;
using System.IO;

namespace WebsitePoller.Workflow
{
    public interface IFileContentComparer : IEqualityComparer<FileInfo>
    {
        bool Equals(string firstPath, string secondPath);
    }
}