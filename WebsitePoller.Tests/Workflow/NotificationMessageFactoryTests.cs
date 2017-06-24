using System;
using System.IO;
using NUnit.Framework;
using WebsitePoller.Workflow;

namespace WebsitePoller.Tests.Workflow
{
    [TestFixture]
    public sealed class NotificationMessageFactoryTests
    {
        public sealed class GetToastXml
        {
            private static IAn An { get; }

            [Test]
            public void ShouldThrowArgumentNullException()
            {
                Assert.Multiple(() => 
                { 
                    Assert.That(() => NotificationMessageFactory.GetToastXml(null, new Uri("http://localhost/")), Throws.ArgumentNullException);
                    Assert.That(() => NotificationMessageFactory.GetToastXml("message", null), Throws.ArgumentNullException);
                });
            }

            [Test]
            public void ShouldProduceCorrectMessage()
            {
                var assemblyPath = An.AssemblyPath();
                var path = Path.Combine(assemblyPath, "Workflow", "toast.xml");
                var content = File.ReadAllText(path);

                var result = NotificationMessageFactory.GetToastXml("message", new Uri("http://localhost/"));
                
                Assert.That(result, Is.Not.Null);
                Assert.That(result.GetXml(), Is.EqualTo(content));
            }
        }
    }
}
