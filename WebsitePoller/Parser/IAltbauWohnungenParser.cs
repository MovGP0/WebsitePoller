using System.Collections.Generic;
using HtmlAgilityPack;
using JetBrains.Annotations;
using WebsitePoller.Entities;

namespace WebsitePoller.Parser
{
    public interface IAltbauWohnungenParser
    {
        [NotNull] IEnumerable<AltbauWohnungInfo> ParseAltbauWohnungenDocumentWithLogging([NotNull]HtmlDocument document);
        [NotNull] IEnumerable<AltbauWohnungInfo> ParseAltbauWohnungenDocument([NotNull]HtmlDocument document);
    }
}