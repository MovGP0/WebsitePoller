using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Serilog;
using WebsitePoller.Setting;

namespace WebsitePoller.Parser
{
    public sealed class AltbauWohnungenParser : IAltbauWohnungenParser
    {
        private static ILogger Log => Serilog.Log.ForContext<AltbauWohnungenParser>();
        private SettingsManager SettingsManager { get; }
        private static readonly Regex AddressRegex 
            = new Regex("(?<postalcode>[0-9]{4}) (?<city>[A-Za-zäöüÄÖÜß ]+), (?<street>.*)");

        public AltbauWohnungenParser(SettingsManager settingsManager)
        {
            SettingsManager = settingsManager;
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
            var candidates = table.Select(row => ParseRow(row.ChildNodes));
            return FilterCandidates(candidates, SettingsManager.Settings);
        }

        private static string FixUriEncoding(string uri)
        {
            return uri.Replace("&amp;", "&").Replace("[", "%5B").Replace("]", "%5D");
        }

        private static AltbauWohnungInfo ParseRow(HtmlNodeCollection nodes)
        {
            var href = FixUriEncoding(nodes[0].QuerySelector("a").Attributes["href"].Value);
            var title = nodes[0].QuerySelector("a").Attributes["title"].Value;
            
            var matches = AddressRegex.Match(title);

            return new AltbauWohnungInfo
            {
                Href = href,
                PostalCode = GetPostalCode(matches),
                City = GetCity(matches),
                Street = GetStreet(matches),
                NumberOfRooms = GetNumberOfRooms(nodes),
                Eigenmittel = GetEigenmittel(nodes),
                MonatlicheKosten = GetMonatlicheKosten(nodes)
            };
        }

        private static decimal GetMonatlicheKosten(HtmlNodeCollection nodes)
        {
            return ParsePrice(nodes[3].InnerHtml);
        }

        private static decimal GetEigenmittel(HtmlNodeCollection nodes)
        {
            return ParsePrice(nodes[2].InnerHtml);
        }

        private static int GetNumberOfRooms(HtmlNodeCollection nodes)
        {
            return int.Parse(nodes[1].QuerySelector("p").InnerHtml);
        }

        private static string GetStreet(Match matches)
        {
            return matches.Groups["street"].Value;
        }

        private static string GetCity(Match matches)
        {
            return matches.Groups["city"].Value;
        }

        private static int GetPostalCode(Match matches)
        {
            return int.Parse(matches.Groups["postalcode"].Value);
        }

        private static decimal ParsePrice(string value)
        {
            var cleanedValue = value.Replace("&euro;", "").Replace(".", "").Replace(",", ".");
            return decimal.Parse(cleanedValue);
        }

        private static IEnumerable<AltbauWohnungInfo> FilterCandidates(IEnumerable<AltbauWohnungInfo> candidates, SettingsBase settings)
        {
            return candidates
                .Where(c => c.Eigenmittel <= settings.MaxEigenmittel)
                .Where(c => c.MonatlicheKosten <= settings.MaxMonatlicheKosten)
                .Where(c => c.NumberOfRooms >= settings.MinNumberOfRooms)
                .Where(c => settings.Cities.Contains(c.City))
                .Where(c => settings.PostalCodes.Contains(c.PostalCode));
        }
    }
}
