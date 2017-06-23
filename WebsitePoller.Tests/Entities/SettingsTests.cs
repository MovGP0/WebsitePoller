using NUnit.Framework;

namespace WebsitePoller.Tests.Entities
{
    [TestFixture]
    public sealed class SettingsTests
    {
        public sealed class EqualsTests
        {
            private IAn An { get; }

            [Test]
            public void EquatingSameSettingsShouldReturnTrue()
            {
                var settings1 = An.Settings();
                var settings2 = An.Settings();

                Assert.That(settings1.Equals(settings2), Is.True);
            }
        }

        public sealed class GetHashCodeTests
        {
            private IAn An { get; }

            [Test]
            public void HashCodeFoSameSettingsShouldBeEqual()
            {
                var hashCode1 = An.Settings().GetHashCode();
                var hashCode2 = An.Settings().GetHashCode();

                Assert.That(hashCode1.Equals(hashCode2), Is.True);
            }
        }
    }
}
