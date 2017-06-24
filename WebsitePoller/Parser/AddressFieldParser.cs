using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Serilog;

namespace WebsitePoller.Parser
{
    public sealed class AddressFieldParser : IAddressFieldParser
    {
        public AddressFieldParserResult ParseWithLoggingOrNull(string addressFieldString)
        {
            try
            {
                return Parse(addressFieldString);
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
                return null;
            }
        }

        [NotNull]
        private static ILogger Log => Serilog.Log.ForContext<AddressFieldParser>();

        private static readonly Regex AddressRegex
            = new Regex("^(?<postalcode>[0-9]{4}) (?<city>[A-Za-z‰ˆ¸ƒ÷‹ﬂ\\. ]+), (?<street>.*)$", RegexOptions.Compiled | RegexOptions.Singleline);

        public AddressFieldParserResult Parse([NotNull]string addressFieldString)
        {
            if (addressFieldString == null) throw new ArgumentNullException(nameof(addressFieldString));
            if (string.IsNullOrWhiteSpace(addressFieldString)) throw new ArgumentOutOfRangeException(nameof(addressFieldString), addressFieldString, "Must not be null or whitespace.");
            if (!AddressRegex.IsMatch(addressFieldString)) throw new FormatException($"Could not parse address of the form '{addressFieldString}'.");

            var matches = AddressRegex.Match(addressFieldString);

            return new AddressFieldParserResult
            {
                PostalCode = GetPostalCode(matches),
                City = GetCity(matches),
                Street = GetStreet(matches),
            };
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
    }
}