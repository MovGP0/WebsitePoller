using AutoMapper;
using DryIoc;
using NUnit.Framework;
using WebsitePoller.Entities;

namespace WebsitePoller.Tests.Mappings
{
    [TestFixture]
    public sealed class SettingsMappingTests
    {
        private static IAn An { get; }
        
        [Test]
        public void ShouldMapSettings()
        {
            var resolver = An.Resolver();
            var mapper = resolver.Resolve<IMapper>();
            var settingsStrings = An.SettingsStrings();
            var expected = An.Settings();

            var settings = mapper.Map<Settings>(settingsStrings);

            Assert.Multiple(() => {
                Assert.That(settings, Is.EqualTo(expected), "Equality");
                Assert.That(settings.From, Is.EqualTo(expected.From), "From");
                Assert.That(settings.Till, Is.EqualTo(expected.Till), "Till");
            });
        }
    }
}
