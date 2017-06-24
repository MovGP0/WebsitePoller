using System.Collections.Generic;
using HtmlAgilityPack;

namespace WebsitePoller.Workflow
{
    public sealed class HtmlDocumentComparer : IEqualityComparer<HtmlDocument>
    {
        public bool Equals(HtmlDocument x, HtmlDocument y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }
        
        public int GetHashCode(HtmlDocument obj)
        {
            return obj == null ? 0 : obj.CheckSum;
        }
    }
}