using System;
using System.Text;
using NUnit.Framework;
using WebsitePoller.FormRegistrator;

namespace WebsitePoller.Tests.FormRegistrator
{
    [TestFixture]
    public sealed class StringBuilderExtensionsTests
    {
        public sealed class AppendParameter
        {
            [Test]
            public void ShouldThrowExceptionWhenStringBuilderIsNull()
            {
                var builder = (StringBuilder) null;
                Assert.Throws<ArgumentNullException>(() => builder.AppendParameter("name", "value"));
            }

            [Test]
            public void ShouldThrowExceptionWhenNameIsNull()
            {
                var builder = new StringBuilder();
                Assert.Throws<ArgumentNullException>(() => builder.AppendParameter(null, "value"));
            }

            [Test]
            public void ShouldThrowExceptionWhenNameIsEmpty()
            {
                var builder = new StringBuilder();
                Assert.Throws<ArgumentException>(() => builder.AppendParameter("   ", "value"));
            }

            [Test]
            public void ShouldThrowExceptionWhenValueIsNull()
            {
                var builder = new StringBuilder();
                Assert.Throws<ArgumentNullException>(() => builder.AppendParameter("name", null));
            }

            [Test]
            public void ShouldReturnSameStringBuilder()
            {
                var builder = new StringBuilder();
                StringBuilder result = null;
                
                try
                {
                    result = builder.AppendParameter("name", "value");
                }
                catch
                {
                    Assert.Inconclusive();
                }

                Assert.That(result, Is.Not.Null);
                Assert.AreSame(builder, result);
            }

            [Test]
            public void ShouldAppendParameter()
            {
                var result = string.Empty;
                const string expected = @"/tx_sozaltbau_pi1[name]=value&tx_sozaltbau_pi1[foo]=bar";

                try
                {
                    var builder = new StringBuilder("/")
                        .AppendParameter("name", "value", true)
                        .AppendParameter("foo", "bar");
                    result = builder.ToString();
                }
                catch
                {
                    Assert.Inconclusive();
                }

                Assert.That(result, Is.EqualTo(expected));
            }
        }
    }
}
