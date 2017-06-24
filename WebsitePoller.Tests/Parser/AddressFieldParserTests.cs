using System;
using NSubstitute;
using NUnit.Framework;
using Serilog;
using WebsitePoller.Parser;

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
            public void MustParseAdresses(string address, int postalCode, string city, string street)
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

            [Test]
            public void MustThrowArgumentNullExceptionWhenStringIsNull()
            {
                var parser = An.AddressFieldParser();
                Assert.That(() => parser.Parse(null), Throws.ArgumentNullException);
            }
            
            [Test]
            [TestCase("")]
            [TestCase("    ")]
            public void MustThrowArgumentOutOfRangeExceptionWhenStringIsEmpty(string address)
            {
                var parser = An.AddressFieldParser();
                Assert.That(() => parser.Parse(address), Throws.InstanceOf<ArgumentOutOfRangeException>());
            }

            [Test]
            [TestCase("foobar")]
            [TestCase("12345 Wien, Straße")]
            [TestCase("1234 Wien ,Straße")]
            [TestCase("1234 Wien Straße")]
            public void MustThrowFormatExceptionWhenAddressHasError(string address)
            {
                var parser = An.AddressFieldParser();
                Assert.That(() => parser.Parse(address), Throws.InstanceOf<FormatException>());
            }
        }

        public sealed class ParseWithLoggingOrNull
        {
            private static IAn An { get; }

            [Test]
            public void Foo()
            {
                var addressLogger = Substitute.For<ILogger>();
                var logger = Substitute.For<ILogger>();
                logger.ForContext<AddressFieldParser>().Returns(addressLogger);
                Log.Logger = logger;

                var parser = An.AddressFieldParser();
                var result = parser.ParseWithLoggingOrNull("");

                Assert.Multiple(() => {
                    Assert.That(result, Is.Null, "result wasn't null");
                    Assert.That(() => logger.DidNotReceive().Error(Arg.Any<Exception>(), Arg.Any<string>()), Throws.Nothing, "global logger received error");
                    Assert.That(() => addressLogger.Received().Error(Arg.Any<ArgumentOutOfRangeException>(), Arg.Any<string>()), Throws.Nothing, "logger did not receive exception");
                });
            }
        }
    }
}
