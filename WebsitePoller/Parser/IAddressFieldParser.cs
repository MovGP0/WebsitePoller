namespace WebsitePoller.Parser
{
    public interface IAddressFieldParser
    {
        AddressFieldParserResult Parse(string addressFieldString);
        AddressFieldParserResult ParseWithLoggingOrNull(string addressFieldString);
    }
}