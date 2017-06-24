using System;
using NSubstitute;
using NUnit.Framework;
using Serilog;
using WebsitePoller.Parser;

namespace WebsitePoller.Tests.Parser
{
    [TestFixture]
    public sealed class AltbauWohnungenRowParserTests
    {
        public sealed class Constructor
        {
            [Test]
            public void ShouldThrowArgumentNullExceptionWhenDependencyIsNull()
            {
                Assert.That(() => new AltbauWohnungenRowParser(null), Throws.ArgumentNullException);
            }

            [Test]
            public void ShouldNotThrowWhenDependencyIsGiven()
            {
                var addressFieldParser = Substitute.For<IAddressFieldParser>();
                Assert.That(() => new AltbauWohnungenRowParser(addressFieldParser), Throws.Nothing);
            }
        }

        public sealed class Parse
        {
            private static IAn An { get; }

            [Test]
            public void ShouldThrowArgumentNullExceptionWhenArgumentIsNull()
            {
                var addressFieldParser = Substitute.For<IAddressFieldParser>();
                var parser = new AltbauWohnungenRowParser(addressFieldParser);
                Assert.That(() => parser.Parse(null), Throws.ArgumentNullException);
            }

            [Test]
            public void ShouldParseHtmlTableRow()
            {
                var rows = An.TableRows();
                var addressFieldParser = Substitute.For<IAddressFieldParser>();
                addressFieldParser.Parse(Arg.Any<string>()).Returns(An.AddressFieldParserResult());
                var parser = new AltbauWohnungenRowParser(addressFieldParser);

                // act 
                var result = parser.Parse(rows);

                // assert
                Assert.That(result, Is.Not.Null, "result was null");
                Assert.Multiple(() =>
                {
                    Assert.That(result.City, Is.EqualTo("Wien"), nameof(result.City));
                    Assert.That(result.Eigenmittel, Is.EqualTo(6506m), nameof(result.Eigenmittel));
                    Assert.That(result.Href, Is.EqualTo(@"nc/home/suche/altbau-wohnungen/?tx_sozaltbau_pi1%5Bad%5D=1&tx_sozaltbau_pi1%5Bmobjnr%5D=7020&tx_sozaltbau_pi1%5Bmlfd%5D=71&cHash=a39630a7a68a5c8145d3ae888be047d4"), nameof(result.Href));
                    Assert.That(result.MonatlicheKosten, Is.EqualTo(1349m), nameof(result.MonatlicheKosten));
                    Assert.That(result.NumberOfRooms, Is.EqualTo(4), nameof(result.NumberOfRooms));
                    Assert.That(result.PostalCode, Is.EqualTo(1010), nameof(result.PostalCode));
                    Assert.That(result.Street, Is.EqualTo("Gürtel Straße 3-2a/16"), nameof(result.Street));
                });
            }

            [Test]
            public void ShouldReturnNullWhenAddressFiledParserReturnsNull()
            {
                var rows = An.TableRows();
                var addressFieldParser = Substitute.For<IAddressFieldParser>();
                addressFieldParser.Parse(Arg.Any<string>()).Returns((AddressFieldParserResult)null);
                var parser = new AltbauWohnungenRowParser(addressFieldParser);

                // act 
                var result = parser.Parse(rows);

                Assert.That(result, Is.Null);
            }
        }

        public sealed class ParseWithLogging
        {
            private static IAn An { get; }

            [Test]
            public void ShouldThrowArgumentNullExceptionWhenArgumentIsNull()
            {
                var addressFieldParser = Substitute.For<IAddressFieldParser>();
                var parser = new AltbauWohnungenRowParser(addressFieldParser);
                Assert.That(() => parser.ParseWithLogging(null), Throws.ArgumentNullException);
            }

            [Test]
            public void ShouldParseHtmlTableRow()
            {
                var rows = An.TableRows();
                var addressFieldParser = Substitute.For<IAddressFieldParser>();
                addressFieldParser.ParseWithLoggingOrNull(Arg.Any<string>()).Returns(An.AddressFieldParserResult());
                var parser = new AltbauWohnungenRowParser(addressFieldParser);

                // act 
                var result = parser.ParseWithLogging(rows);

                // assert
                Assert.That(result, Is.Not.Null, "result was null");
                Assert.Multiple(() =>
                {
                    Assert.That(result.City, Is.EqualTo("Wien"), nameof(result.City));
                    Assert.That(result.Eigenmittel, Is.EqualTo(6506m), nameof(result.Eigenmittel));
                    Assert.That(result.Href, Is.EqualTo(@"nc/home/suche/altbau-wohnungen/?tx_sozaltbau_pi1%5Bad%5D=1&tx_sozaltbau_pi1%5Bmobjnr%5D=7020&tx_sozaltbau_pi1%5Bmlfd%5D=71&cHash=a39630a7a68a5c8145d3ae888be047d4"), nameof(result.Href));
                    Assert.That(result.MonatlicheKosten, Is.EqualTo(1349m), nameof(result.MonatlicheKosten));
                    Assert.That(result.NumberOfRooms, Is.EqualTo(4), nameof(result.NumberOfRooms));
                    Assert.That(result.PostalCode, Is.EqualTo(1010), nameof(result.PostalCode));
                    Assert.That(result.Street, Is.EqualTo("Gürtel Straße 3-2a/16"), nameof(result.Street));
                });
            }

            [Test]
            public void ShouldLogExceptions()
            {
                // logger 
                var logger = Substitute.For<ILogger>();
                var parserLogger = Substitute.For<ILogger>();
                logger.ForContext<AltbauWohnungenRowParser>().Returns(parserLogger);
                Log.Logger = logger;

                var rows = An.TableRows();
                var addressFieldParser = Substitute.For<IAddressFieldParser>();
                addressFieldParser.ParseWithLoggingOrNull(Arg.Any<string>()).Returns(x => throw new Exception());
                var parser = new AltbauWohnungenRowParser(addressFieldParser);

                // act 
                var result = parser.ParseWithLogging(rows);

                Assert.Multiple(() =>
                {
                    Assert.That(result, Is.Null, "result was not null");
                    Assert.That(() => parserLogger.Received().Error(Arg.Any<Exception>(), Arg.Any<string>()), Throws.Nothing, "did not log exception");
                });
            }
        }
    }
}
