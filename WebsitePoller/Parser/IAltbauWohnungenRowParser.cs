using HtmlAgilityPack;
using JetBrains.Annotations;
using WebsitePoller.Entities;

namespace WebsitePoller.Parser
{
    public interface IAltbauWohnungenRowParser
    {
        AltbauWohnungInfo Parse([NotNull]HtmlNodeCollection nodes);
        AltbauWohnungInfo ParseWithLogging([NotNull]HtmlNodeCollection nodes);
    }
}