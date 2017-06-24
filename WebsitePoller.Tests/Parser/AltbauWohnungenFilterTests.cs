using System.Linq;
using NUnit.Framework;
using WebsitePoller.Parser;
using WebsitePoller.Setting;

namespace WebsitePoller.Tests.Parser
{
    [TestFixture]
    public sealed class AltbauWohnungenFilterTests
    {
        public sealed class Constructor
        {
            private static IAn An { get; }

            [Test]
            public void ConstructorWithNullArgumentMustThrowArgumentNullException()
            {
                Assert.That(() => new AltbauWohnungenFilter(null), Throws.ArgumentNullException);
            }

            [Test]
            public void ConstructorWithSettingsManagerMustNotThrow()
            {
                var settingsManager = An.SettingsManager();
                Assert.That(() => new AltbauWohnungenFilter(settingsManager), Throws.Nothing);
            }
        }

        public sealed class Filter
        {
            private static IAn An { get; }

            [Test]
            public void MustThrowInvalidOperationExceptionWhenSettingsIsNotYetSet()
            {
                var settingsManager = new SettingsManager();
                var infos = new []
                {
                    An.AltbauWohnungInfo()
                };

                var filter = new AltbauWohnungenFilter(settingsManager);
                Assert.That(() => filter.Filter(infos), Throws.InvalidOperationException);
            }

            [Test]
            public void MustFilterResults()
            {
                var settingsManager = An.SettingsManager();
                settingsManager.Settings = An.Settings();

                var info1 = An.AltbauWohnungInfo();
                var info2 = An.AltbauWohnungInfo2();

                var infos = new[]
                {
                    info1,
                    info2,
                    info1
                };

                var filter = new AltbauWohnungenFilter(settingsManager);
                var result = filter.Filter(infos);

                Assert.That(result, Is.Not.Null, "result was null");
                Assert.That(result.Count(), Is.EqualTo(1), "result was not properly filtered");
                Assert.That(result.First(), Is.EqualTo(info2));
            }
        }
    }
}
