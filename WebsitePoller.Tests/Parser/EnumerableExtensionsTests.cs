using System.Linq;
using NUnit.Framework;
using WebsitePoller.Parser;

namespace WebsitePoller.Tests.Parser
{
    [TestFixture]
    public sealed class EnumerableExtensionsTests
    {
        public sealed class WithoutNull
        {
            private class Person {}

            [Test]
            public void ShouldFilterNullValues()
            {
                var numbers = new []
                {
                    new Person(), null, new Person(), null, new Person(), new Person(), null, null
                };

                var result = numbers.WithoutNull();
                Assert.That(result.Count(), Is.EqualTo(4));
            }
        }
    }
}
