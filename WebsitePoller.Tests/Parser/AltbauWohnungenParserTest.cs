using System;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Serilog;
using WebsitePoller.Parser;

namespace WebsitePoller.Tests.Parser
{
    [TestFixture]
    public sealed class AltbauWohnungenParserTest
    {
        public sealed class Constructor
        {
            [Test]
            public void ShouldThrowArgumentNullExceptionWhenArgumentIsNull()
            {
                Assert.That(() => new AltbauWohnungenParser(null), Throws.ArgumentNullException);
            }

            [Test]
            public void ShouldNotThrowArgumentNullExceptionWhenArgumentIsSubstitute()
            {
                var rowParser = Substitute.For<IAltbauWohnungenRowParser>();
                Assert.That(() => new AltbauWohnungenParser(rowParser), Throws.Nothing);
            }
        }

        public sealed class ParseAltbauWohnungenDocument
        {
            private static IAn An { get; }

            [Test]
            public void MustThrowArgumentNullExceptionWhenArgumentIsNull()
            {
                var rowParser = Substitute.For<IAltbauWohnungenRowParser>();
                var parser = new AltbauWohnungenParser(rowParser);

                Assert.That(() => parser.ParseAltbauWohnungenDocument(null), Throws.ArgumentNullException);
            }

            [Test]
            public void ShouldParseDocument()
            {
                // setup parser
                var rowParser = Substitute.For<IAltbauWohnungenRowParser>();
                rowParser.Parse(Arg.Any<HtmlNodeCollection>()).Returns(An.AltbauWohnungInfo());
                var parser = new AltbauWohnungenParser(rowParser);

                // load document
                var assemblyPath = An.AssemblyPath();
                var documentPath = Path.Combine(assemblyPath, "Parser", "altbau-wohnungen.html");
                var document = new HtmlDocument();
                using (var stream = File.OpenRead(documentPath))
                {
                    document.Load(stream);
                }

                // act
                var result = parser.ParseAltbauWohnungenDocument(document);

                Assert.That(result, Is.Not.Null, "result was null");
                Assert.That(result.Count(), Is.EqualTo(4), "result count");
                Assert.That(() => rowParser.Received(4).Parse(Arg.Any<HtmlNodeCollection>()), Throws.Nothing);
            }
        }

        public sealed class ParseAltbauWohnungenDocumentWithLogging
        {
            private static IAn An { get; }

            [Test]
            public void MustThrowArgumentNullExceptionWhenArgumentIsNull()
            {
                var rowParser = Substitute.For<IAltbauWohnungenRowParser>();
                var parser = new AltbauWohnungenParser(rowParser);

                Assert.That(() => parser.ParseAltbauWohnungenDocumentWithLogging(null), Throws.ArgumentNullException);
            }

            [Test]
            public void ShouldParseDocument()
            {
                // setup logging
                var logger = Substitute.For<ILogger>();
                var parserLogger = Substitute.For<ILogger>();
                logger.ForContext<AltbauWohnungenParser>().Returns(parserLogger);


                // setup parser
                var rowParser = Substitute.For<IAltbauWohnungenRowParser>();
                rowParser.ParseWithLogging(Arg.Any<HtmlNodeCollection>()).Returns(An.AltbauWohnungInfo());
                var parser = new AltbauWohnungenParser(rowParser);

                // load document
                var assemblyPath = An.AssemblyPath();
                var documentPath = Path.Combine(assemblyPath, "Parser", "altbau-wohnungen.html");
                var document = new HtmlDocument();
                using (var stream = File.OpenRead(documentPath))
                {
                    document.Load(stream);
                }

                // act
                var result = parser.ParseAltbauWohnungenDocumentWithLogging(document);

                Assert.That(result, Is.Not.Null, "result was null");
                Assert.That(result.Count(), Is.EqualTo(4), "result count");
                Assert.That(() => rowParser.Received(4).ParseWithLogging(Arg.Any<HtmlNodeCollection>()), Throws.Nothing);
                Assert.That(() => parserLogger.DidNotReceive().Error(Arg.Any<Exception>(), Arg.Any<string>()), Throws.Nothing);
            }

            [Test]
            public void ShouldLogErrors()
            {
                // setup logging
                var logger = Substitute.For<ILogger>();
                var parserLogger = Substitute.For<ILogger>();
                logger.ForContext<AltbauWohnungenParser>().Returns(parserLogger);
                Log.Logger = logger;

                // setup parser
                var rowParser = Substitute.For<IAltbauWohnungenRowParser>();
                rowParser.ParseWithLogging(Arg.Any<HtmlNodeCollection>()).Returns(x => throw new Exception());
                var parser = new AltbauWohnungenParser(rowParser);

                // load document
                var assemblyPath = An.AssemblyPath();
                var documentPath = Path.Combine(assemblyPath, "Parser", "altbau-wohnungen.html");
                var document = new HtmlDocument();
                using (var stream = File.OpenRead(documentPath))
                {
                    document.Load(stream);
                }

                // act
                var result = parser.ParseAltbauWohnungenDocumentWithLogging(document).ToArray();

                // assert
                Assert.Multiple(() => 
                {
                    Assert.That(result, Is.Not.Null, "result was null");
                    Assert.That(result.Count, Is.EqualTo(0), "result count");
                    Assert.That(() => rowParser.Received(1).ParseWithLogging(Arg.Any<HtmlNodeCollection>()), Throws.Nothing, "row parser was not called");
                    Assert.That(() => parserLogger.Received().Error(Arg.Any<Exception>(), Arg.Any<string>()), Throws.Nothing, "error was not logged");
                });
            }
        }
    }
}
