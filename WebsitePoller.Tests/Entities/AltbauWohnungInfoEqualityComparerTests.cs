using NUnit.Framework;

namespace WebsitePoller.Tests.Entities
{
    [TestFixture]
    public sealed class AltbauWohnungInfoEqualityComparerTests
    {
        public sealed class GetHashCodeTests
        {
            private static IAn An { get; }

            [Test]
            public void GettingHashCodeShouldBeDeterministic()
            {
                var info1 = An.AltbauWohnungInfo();
                var comparer = An.AltbauWohnungInfoEqualityComparer();
                var hashCode = comparer.GetHashCode(info1);

                Assert.That(hashCode, Is.EqualTo(-1810387702));
            }
        }

        public sealed class EqualsTests
        {
            private static IAn An { get; }

            [Test]
            public void SameAltbauWohnungInfosShouldBeEqual()
            {
                var info1 = An.AltbauWohnungInfo();
                var info2 = An.AltbauWohnungInfo();
                var comparer = An.AltbauWohnungInfoEqualityComparer();

                Assert.That(comparer.Equals(info1, info2), Is.True);
            }

            [Test]
            public void ShouldCompareWithNull()
            {
                var info = An.AltbauWohnungInfo();
                var comparer = An.AltbauWohnungInfoEqualityComparer();

                Assert.Multiple(() => {
                    Assert.That(comparer.Equals(info, null), Is.False);
                    Assert.That(comparer.Equals(null, info), Is.False);
                    Assert.That(comparer.Equals(null, null), Is.True);
                });
            }
        }
    }
}
