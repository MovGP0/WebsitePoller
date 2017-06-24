using System.Reflection;
using NUnit.Framework;
using WebsitePoller.Workflow;

namespace WebsitePoller.Tests.Workflow
{
    [TestFixture]
    public sealed class AssemblyExtensionsTests
    {
        public sealed class GetDirectoryPath
        {
            [Test]
            public void ShouldThrowArgumentNullException()
            {
                Assert.That(() => AssemblyExtensions.GetDirectoryPath(null), Throws.ArgumentNullException);
            }

            [Test]
            public void ShouldNotReturnEmptyPath()
            {
                var assembly = Assembly.GetExecutingAssembly();
                var path = assembly.GetDirectoryPath();
                Assert.That(path, Is.Not.Empty);
            }
        }
    }
}
