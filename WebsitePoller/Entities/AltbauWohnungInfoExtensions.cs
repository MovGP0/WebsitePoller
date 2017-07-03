using System.Collections.Generic;
using System.Linq;
using WebsitePoller.Workflow;

namespace WebsitePoller.Entities
{
    public static class AltbauWohnungInfoExtensions
    {
        public static IEnumerable<AltbauWohnungInfo> FilterNewOffers(this IEnumerable<AltbauWohnungInfo> offers, string postedHrefsFilePath)
        {
            var hrefs = FileHelper.GetFileLines(postedHrefsFilePath).ToArray();
            return offers.Where(o => !hrefs.Contains(o.Href));
        }
    }
}