using System;
using NUnit.Framework;
using WebsitePoller.Workflow;

namespace WebsitePoller.Tests.Workflow
{
    [TestFixture]
    public sealed class UriExtensionsTests
    {
        public sealed class GetDomain
        {
            [Test]
            public void ShouldThrowExceptionWhenUriIsNull()
            {
                Assert.That(() => UriExtensions.GetDomain(null), Throws.ArgumentNullException);
            }

            [Test]
            [TestCase("http://www.example.org/wiki/foo/bar&bla;bla", "http://www.example.org/")]
            [TestCase("https://xxx.foo.com/&bla=bla", "https://xxx.foo.com/")]
            [TestCase("https://user:password@xxx.foo.com:32", "https://xxx.foo.com:32/")]
            public void ShouldReturnDomain(string value, string expected)
            {
                var uri = new Uri(value);
                var result = uri.GetDomain();
                Assert.That(result.ToString(), Is.EqualTo(expected));
            }
        }
    }
}
