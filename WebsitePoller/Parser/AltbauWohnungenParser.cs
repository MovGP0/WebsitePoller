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
        private static ILogger Log => Serilog.Log.ForContext<AltbauWohnungenParser>();
        private IAddressFieldParser AddressFieldParser { get; }

        public AltbauWohnungenParser([NotNull]IAddressFieldParser addressFieldParser)
        {
            AddressFieldParser = addressFieldParser ?? throw new ArgumentNullException(nameof(addressFieldParser));
        }

        public IEnumerable<AltbauWohnungInfo> ParseAltbauWohnungenDocumentWithLogging(HtmlDocument document)
        {
            try
            {
                return ParseAltbauWohnungenDocument(document);
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
                return new List<AltbauWohnungInfo>();
            }
        }

        public IEnumerable<AltbauWohnungInfo> ParseAltbauWohnungenDocument(HtmlDocument document)
        {
            var table = document.QuerySelectorAll("table.contenttable > tbody > tr");
            return table.Select(row => ParseRow(row.ChildNodes)).WithoutNull();
        }

        private static string FixUriEncoding(string uri)
        {
            return uri.Replace("&amp;", "&").Replace("[", "%5B").Replace("]", "%5D");
        }

        private AltbauWohnungInfo ParseRow(HtmlNodeCollection nodes)
        {
            var href = FixUriEncoding(nodes[0].QuerySelector("a").Attributes["href"].Value);
            var title = nodes[0].QuerySelector("a").Attributes["title"].Value;
            var address = AddressFieldParser.ParseWithLoggingOrNull(title);

            if (address == null)
            {
                return null;
            }

            return new AltbauWohnungInfo
            {
                Href = href,
                PostalCode = address.PostalCode,
                City = address.City,
                Street = address.Street,
                NumberOfRooms = GetNumberOfRooms(nodes),
                Eigenmittel = GetEigenmittel(nodes),
                MonatlicheKosten = GetMonatlicheKosten(nodes)
            };
        }

        private static decimal GetMonatlicheKosten(HtmlNodeCollection nodes)
        {
            return PriceFieldParser.Parse(nodes[3].InnerHtml);
        }

        private static decimal GetEigenmittel(HtmlNodeCollection nodes)
        {
            return PriceFieldParser.Parse(nodes[2].InnerHtml);
        }

        private static int GetNumberOfRooms(HtmlNodeCollection nodes)
        {
            return int.Parse(nodes[1].QuerySelector("p").InnerHtml);
        }
    }
}
