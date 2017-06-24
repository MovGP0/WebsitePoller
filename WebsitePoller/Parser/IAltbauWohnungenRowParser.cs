using HtmlAgilityPack;
using WebsitePoller.Entities;

namespace WebsitePoller.Parser
{
    public interface IAltbauWohnungenRowParser
    {
        AltbauWohnungInfo Parse(HtmlNodeCollection nodes);
        AltbauWohnungInfo ParseWithLogging(HtmlNodeCollection nodes);
    }
}