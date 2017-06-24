using System;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Serilog;
using WebsitePoller.Entities;

namespace WebsitePoller.Parser
{
    public sealed class AltbauWohnungenRowParser : IAltbauWohnungenRowParser
    {
        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext<AltbauWohnungenParser>();

        [NotNull]
        private IAddressFieldParser AddressFieldParser { get; }

        public AltbauWohnungenRowParser([NotNull]IAddressFieldParser addressFieldParser)
        {
            AddressFieldParser = addressFieldParser ?? throw new ArgumentNullException(nameof(addressFieldParser));
        }

        public AltbauWohnungInfo ParseWithLogging(HtmlNodeCollection nodes)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));

            try
            {
                return Parse(nodes);
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
                return null;
            }
        }

        public AltbauWohnungInfo Parse(HtmlNodeCollection nodes)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));

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

        private static string FixUriEncoding(string uri)
        {
            return uri.Replace("&amp;", "&").Replace("[", "%5B").Replace("]", "%5D");
        }
    }
}