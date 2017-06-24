using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using WebsitePoller.Entities;

namespace WebsitePoller.Tests.Entities
{
    [TestFixture]
    public sealed class SettingsStringsTests
    {
        private static IAn An { get; }
        
        [Test]
        public void Serialize()
        {
            
            var path = Path.Combine(An.AssemblyPath(), "Entities", "SettingsStrings.json");
            var expected = File.ReadAllText(path);
            var settings = An.SettingsStrings();

            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            Assert.That(json, Is.EqualTo(expected));
        }

        [Test]
        public void Deserialize()
        {
            var path = Path.Combine(An.AssemblyPath(), "Entities", "SettingsStrings.json");
            var json = File.ReadAllText(path);
            var expected = An.SettingsStrings();

            var info = JsonConvert.DeserializeObject<SettingsStrings>(json);
            Assert.That(info, Is.EqualTo(expected));
        }
    }
}
