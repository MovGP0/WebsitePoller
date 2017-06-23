using NUnit.Framework;

namespace WebsitePoller.Tests.Entities
{
    [TestFixture]
    public static class VersionFromTillEqualityComparerTests
    {
        public sealed class EqualsTests
        {
            private static IAn An { get; }

            [Test]
            public void ShouldCompareReferences()
            {
                var comparer = An.VersionFromTillEqualityComparer();
                var settings = An.SettingsStrings();

                Assert.Multiple(() => { 
                    Assert.That(comparer.Equals(settings, settings), Is.True);
                    Assert.That(comparer.Equals(settings, null), Is.False);
                    Assert.That(comparer.Equals(null, settings), Is.False);
                    Assert.That(comparer.Equals(null, null), Is.True);
                });
            }
        }

        public sealed class GetHashCodeTests
        {
            private static IAn An { get; }

            [Test]
            public void HashCodesShouldBeConsistent()
            {
                var comparer = An.VersionFromTillEqualityComparer();
                var settings = An.SettingsStrings();
                
                Assert.That(comparer.GetHashCode(settings), Is.EqualTo(1186175842));
            }
        }
    }
}