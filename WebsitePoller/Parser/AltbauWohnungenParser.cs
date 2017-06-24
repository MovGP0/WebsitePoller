using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Serilog;
using WebsitePoller.Entities;

namespace WebsitePoller.Parser
{
    public sealed class AltbauWohnungenParser : IAltbauWohnungenParser
    {
        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext<AltbauWohnungenParser>();
        
        [NotNull]
        private IAltbauWohnungenRowParser AltbauWohnungenRowParser { get; }

        public AltbauWohnungenParser([NotNull] IAltbauWohnungenRowParser altbauWohnungenRowParser)
        {
            AltbauWohnungenRowParser = altbauWohnungenRowParser ?? throw new ArgumentNullException(nameof(altbauWohnungenRowParser));
        }

        public IEnumerable<AltbauWohnungInfo> ParseAltbauWohnungenDocumentWithLogging(HtmlDocument document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            try
            {
                return GetContentTable(document)
                    .Select(row => AltbauWohnungenRowParser.ParseWithLogging(row.ChildNodes))
                    .WithoutNull()
                    .ToArray();
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
                return new List<AltbauWohnungInfo>();
            }
        }
        
        public IEnumerable<AltbauWohnungInfo> ParseAltbauWohnungenDocument(HtmlDocument document)
        {
            if(document == null) throw new ArgumentNullException(nameof(document));

            return GetContentTable(document)
                .Select(row => AltbauWohnungenRowParser.Parse(row.ChildNodes))
                .WithoutNull();
        }

        [NotNull]
        private static IEnumerable<HtmlNode> GetContentTable([NotNull]HtmlDocument document)
        {
            return document.QuerySelectorAll("table.contenttable > tbody > tr");
        }
    }
}
