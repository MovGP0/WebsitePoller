using NUnit.Framework;

namespace WebsitePoller.Tests.Workflow
{
    [TestFixture]
    public sealed class HtmlDocumentComparerTests
    {
        public sealed class GetHashCodeTests
        {
            private IAn An { get; }

            [Test]
            public void HashShouldBeDeterministic()
            {
                var document1 = An.HtmlDocument();
                var document2 = An.HtmlDocument();
                var htmlDocumentComparer = An.HtmlDocumentComparer();

                var hash1 = htmlDocumentComparer.GetHashCode(document1);
                var hash2 = htmlDocumentComparer.GetHashCode(document2);

                Assert.Multiple(() =>
                {
                    Assert.That(hash1, Is.EqualTo(1207631242), "hash has changed");
                    Assert.That(hash1, Is.EqualTo(hash2), "hashes are not deterministic");
                });
            }

            [Test]
            public void ShouldReturnZeroForNullObjects()
            {
                var htmlDocumentComparer = An.HtmlDocumentComparer();
                var hash = htmlDocumentComparer.GetHashCode(null);
                Assert.That(hash, Is.EqualTo(0));
            }
        }

        public sealed class EqualsTests
        {
            private IAn An { get; }

            [Test]
            public void DocumentWithSameContentShouldBeEqual()
            {
                var document1 = An.HtmlDocument();
                var document2 = An.HtmlDocument();
                var htmlDocumentComparer = An.HtmlDocumentComparer();

                var result = htmlDocumentComparer.Equals(document1, document2);

                Assert.That(result, Is.True);
            }

            [Test]
            public void ComparingWithNullShouldReturnFalse()
            {
                var document = An.HtmlDocument();
                var htmlDocumentComparer = An.HtmlDocumentComparer();

                var result0 = htmlDocumentComparer.Equals(document, document);
                var result1 = htmlDocumentComparer.Equals(document, null);
                var result2 = htmlDocumentComparer.Equals(null, document);
                var result3 = htmlDocumentComparer.Equals(null, null);

                Assert.Multiple(() => 
                {
                    Assert.That(result0, Is.True, "Comparing equal references.");
                    Assert.That(result1, Is.False, "Comparing with null on second place.");
                    Assert.That(result2, Is.False, "Comaring with null on first place.");
                    Assert.That(result3, Is.True, "Comaring with two nulls.");
                });
            }

            [Test]
            public void DocumentWithDifferentContentShouldBeEqual()
            {
                var document1 = An.HtmlDocument();
                var document2 = An.EmptyHtmlDocument();
                
                var htmlDocumentComparer = An.HtmlDocumentComparer();

                var result = htmlDocumentComparer.Equals(document1, document2);

                Assert.That(result, Is.False);
            }
        }
    }
}

