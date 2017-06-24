using System;
using NSubstitute;
using NUnit.Framework;
using Serilog;
using WebsitePoller.Parser;

namespace WebsitePoller.Tests.Parser
{
    [TestFixture]
    public sealed class PriceFieldParserTests
    {
        public sealed class Parse
        {
            [Test]
            [TestCase("&euro;12.345,67", 12345.67)]
            [TestCase("&euro; 126,00", 126.00)]
            [TestCase("123,45 &euro;", 123.45)]
            [TestCase("&euro; 12 345,67", 12345.67)]
            public void ShouldParseValue(string value, decimal expected)
            {
                var result = PriceFieldParser.Parse(value);
                Assert.That(result, Is.EqualTo(expected));
            }

            [Test]
            [TestCase(null)]
            [TestCase("")]
            [TestCase("    ")]
            public void ShouldThrowExceptionOnInvalidFormat(string value)
            {
                Assert.That(() => PriceFieldParser.Parse(value), Throws.Exception);
            }
        }
        
        public sealed class ParseWithLogging
        {
            [Test]
            [TestCase(null)]
            [TestCase("")]
            [TestCase("   ")]
            [TestCase("foobar")]
            public void ShouldLogError(string value)
            {
                var logger = Substitute.For<ILogger>();
                var parserLogger = Substitute.For<ILogger>();
                logger.ForContext<PriceFieldParser>().Returns(parserLogger);
                Log.Logger = logger;

                var result = PriceFieldParser.ParseWithLogging(value);
                
                Assert.Multiple(() => {
                    Assert.That(result, Is.EqualTo(0m));
                    Assert.That(() => parserLogger.Received().Error(Arg.Any<Exception>(), Arg.Any<string>()), Throws.Nothing);
                });
            }
        }
    }
}
