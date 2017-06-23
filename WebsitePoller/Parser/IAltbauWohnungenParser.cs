using System.Collections.Generic;
using HtmlAgilityPack;

namespace WebsitePoller.Parser
{
    public interface IAltbauWohnungenParser
    {
        IEnumerable<AltbauWohnungInfo> ParseAltbauWohnungenDocumentWithLogging(HtmlDocument document);
        IEnumerable<AltbauWohnungInfo> ParseAltbauWohnungenDocument(HtmlDocument document);
    }
}