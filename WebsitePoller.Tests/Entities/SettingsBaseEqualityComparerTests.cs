using NUnit.Framework;

namespace WebsitePoller.Tests.Entities
{
    [TestFixture]
    public sealed class SettingsBaseEqualityComparerTests
    {
        public sealed class EqualsTests
        {
            private IAn An { get; }

            [Test]
            public void ShouldEqualsIdenticalSettings()
            {
                var settingStrings1 = An.SettingsStrings();
                var comparer = An.SettingsBaseEqualityComparer();
                
                Assert.That(comparer.Equals(settingStrings1, settingStrings1), Is.True);
            }

            [Test]
            public void ShouldEqualSameSettings()
            {
                var settingStrings1 = An.SettingsStrings();
                var settingStrings2 = An.SettingsStrings();
                var comparer = An.SettingsBaseEqualityComparer();

                Assert.That(comparer.Equals(settingStrings1, settingStrings2), Is.True);
            }
        }

        public sealed class GetHashCodeTests
        {
            private IAn An { get; }

            [Test]
            public void HashCodeForSameSettingsShouldBeDeterministic()
            {
                var settingStrings1 = An.SettingsStrings();
                
                var comparer = An.SettingsBaseEqualityComparer();

                var hashCode1 = comparer.GetHashCode(settingStrings1);
                var hashCode2 = comparer.GetHashCode(settingStrings1);

                Assert.That(hashCode1, Is.EqualTo(hashCode2));
            }

            [Test]
            public void HashCodeForSameSettingsShouldBeEqual()
            {
                var settingStrings1 = An.SettingsStrings();
                var settingStrings2 = An.SettingsStrings();

                var comparer = An.SettingsBaseEqualityComparer();

                var hashCode1 = comparer.GetHashCode(settingStrings1);
                var hashCode2 = comparer.GetHashCode(settingStrings2);

                Assert.That(hashCode1, Is.EqualTo(hashCode2));
            }
        }
    }
}