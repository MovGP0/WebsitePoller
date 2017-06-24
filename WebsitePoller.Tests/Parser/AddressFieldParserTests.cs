using NUnit.Framework;

namespace WebsitePoller.Tests.Parser
{
    [TestFixture]
    public sealed class AddressFieldParserTests
    {
        public sealed class Parse
        {
            private static IAn An { get; }

            [Test]
            [TestCase("1110 Wien, Trinkhausstraße 11-13", 1110, "Wien", "Trinkhausstraße 11-13")]
            [TestCase("1210 Wien, Wenhartgasse 34", 1210, "Wien", "Wenhartgasse 34")]
            [TestCase("3426 Muckendorf, Bahnstraße 12-14", 3426, "Muckendorf", "Bahnstraße 12-14")]
            [TestCase("3430 Tulln, Rudolf-Buchinger-Str.34a+b", 3430, "Tulln", "Rudolf-Buchinger-Str.34a+b")]
            [TestCase("2700 Wr. Neustadt, Wiener Str. 34/2/9", 2700, "Wr. Neustadt", "Wiener Str. 34/2/9")]
            public void Foo(string address, int postalCode, string city, string street)
            {
                var parser = An.AddressFieldParser();
                var result = parser.Parse(address);
                Assert.Multiple(() =>
                {
                    Assert.That(result.PostalCode, Is.EqualTo(postalCode), nameof(postalCode));
                    Assert.That(result.City, Is.EqualTo(city), nameof(city));
                    Assert.That(result.Street, Is.EqualTo(street), nameof(street));
                });
            }
        }
    }
}
