using System.Collections.Generic;
using HtmlAgilityPack;

namespace WebsitePoller.Workflow
{
    public sealed class HtmlDocumentComparer : IEqualityComparer<HtmlDocument>
    {
        public bool Equals(HtmlDocument x, HtmlDocument y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            
            if(GetHashCode(x) != GetHashCode(y)) return false;

            var xHtml = x.DocumentNode.InnerHtml;
            var yHtml = y.DocumentNode.InnerHtml;
            return xHtml == yHtml;
        }
        
        public int GetHashCode(HtmlDocument obj)
        {
            return obj?.DocumentNode.InnerHtml.GetHashCode() ?? 0;
        }
    }
}