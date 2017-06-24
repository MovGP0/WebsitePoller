using System.Collections.Generic;
using WebsitePoller.Entities;

namespace WebsitePoller.Parser
{
    public interface IAltbauWohnungenFilter
    {
        IEnumerable<AltbauWohnungInfo> Filter(IEnumerable<AltbauWohnungInfo> altbauWohnungInfos);
    }
}