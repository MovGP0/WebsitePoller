using System;
using Serilog;

namespace WebsitePoller.Parser
{
    public sealed class PriceFieldParser
    {
        private static ILogger Log => Serilog.Log.ForContext<PriceFieldParser>();

        public static decimal ParseWithLogging(string value)
        {
            try
            {
                return Parse(value);
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
                return 0m;
            }
        }

        public static decimal Parse(string value)
        {
            var cleanedValue = value.Replace("&euro;", "").Replace(".", "").Replace(",", ".").Replace(" ", "");
            return decimal.Parse(cleanedValue);
        }
    }
}