using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using WebsitePoller.Workflow;

namespace WebsitePoller.Tests.Workflow
{
    [TestFixture]
    public sealed class HtmlDocumentFactoryTests
    {
        public sealed class FromFileAsync
        {
            private static IAn An { get; }

            [Test]
            public void ShouldThrowArgumentNullExceptionWhenPathIsNull()
            {
                Assert.That(() => HtmlDocumentFactory.FromFileAsync(null), Throws.ArgumentNullException);
            }

            [Test]
            public void ShouldThrowFileNotFoundExceptionWhenPathIsInvalid()
            {
                Assert.That(async() => await HtmlDocumentFactory.FromFileAsync("foobar.dat"), Throws.InstanceOf<FileNotFoundException>());
            }

            [Test]
            public async Task ShouldReturnFileWhenFound()
            {
                var assemblyPath = An.AssemblyPath();
                var path = Path.Combine(assemblyPath, "altbau-wohnungen.html");

                var result = await HtmlDocumentFactory.FromFileAsync(path);

                Assert.Multiple(() => { 
                    Assert.That(result, Is.Not.Null, "was null");
                    Assert.That(result.DocumentNode.InnerHtml.GetHashCode(), Is.EqualTo(1207631242), "hash code");
                });
            }
        }
    }
}
