using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using WebsitePoller.Entities;

namespace WebsitePoller.Tests.Entities
{
    [TestFixture]
    public sealed class AltbauWohnungInfoTests
    {
        private static IAn An { get; }
        
        [Test]
        public void Serialize()
        {
            var path = Path.Combine(An.AssemblyPath(), "Entities", "AltbauWohnungInfo.json");
            var expected = File.ReadAllText(path);
            var info = An.AltbauWohnungInfo();

            var json = JsonConvert.SerializeObject(info, Formatting.Indented);
            Assert.That(json, Is.EqualTo(expected));
        }

        [Test]
        public void Deserialize()
        {
            var path = Path.Combine(An.AssemblyPath(), "Entities", "AltbauWohnungInfo.json");
            var json = File.ReadAllText(path);
            var expected = An.AltbauWohnungInfo();

            var info = JsonConvert.DeserializeObject<AltbauWohnungInfo>(json);
            Assert.That(info, Is.EqualTo(expected));
        }
    }
}
