using NUnit.Framework;

namespace WebsitePoller.Tests.Entities
{
    [TestFixture]
    public sealed class PostalAddressTests
    {
        public sealed class EqualsTests
        {
            private IAn An { get; }

            [Test]
            public void EquatingSamePostalAddressShouldReturnTrue()
            {
                var settings1 = An.PostalAddress();
                var settings2 = An.PostalAddress();

                Assert.That(settings1.Equals(settings2), Is.True);
            }
        }

        public sealed class GetHashCodeTests
        {
            private IAn An { get; }

            [Test]
            public void HashCodeFoSamePostalAddressShouldBeEqual()
            {
                var hashCode1 = An.PostalAddress().GetHashCode();
                var hashCode2 = An.PostalAddress().GetHashCode();

                Assert.That(hashCode1.Equals(hashCode2), Is.True);
            }
        }
    }
}