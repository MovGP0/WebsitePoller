using System;
using NodaTime;
using NUnit.Framework;
using WebsitePoller.Mappings;

namespace WebsitePoller.Tests.Mappings
{
    [TestFixture]
    public sealed class LocalTimeConverterTests
    {
        public sealed class Convert
        {
            [Test]
            public void ShouldNotThrowWhenConstructed()
            {
                Assert.That(() => new LocalTimeConverter(), Throws.Nothing);
            }

            [Test]
            [TestCase(null)]
            [TestCase("")]
            [TestCase("    ")]
            public void ShouldThrowExceptionWhenSourceIsNullOrWhiteSpace(string source)
            {
                var converter = new LocalTimeConverter();
                Assert.That(() => converter.Convert(source, new LocalTime(), null), Throws.InstanceOf<ArgumentOutOfRangeException>());
            }

            [Test]
            [TestCase("00", 00, 00, 00)]
            [TestCase("00:00:00", 00, 00, 00)]
            [TestCase("0:0:0", 00, 00, 00)]
            [TestCase("1:2:3", 1, 2, 3)]
            [TestCase("12", 12, 00, 00)]
            [TestCase("12:34", 12, 34, 00)]
            [TestCase("12:34:56", 12, 34, 56)]
            [TestCase("23:59:59", 23, 59, 59)]
            public void ShouldParseDateTimeStringProperly(string source, int hours, int minutes, int seconds)
            {
                LocalTimeConverter converter = null;
                try
                {
                    converter = new LocalTimeConverter();
                }
                catch
                {
                    Assert.Inconclusive();
                }
                
                var localTime = converter.Convert(source, new LocalTime(), null);

                Assert.That(localTime.Hour, Is.EqualTo(hours));
                Assert.That(localTime.Minute, Is.EqualTo(minutes));
                Assert.That(localTime.Second, Is.EqualTo(seconds));
            }

            [Test]
            [TestCase("24:00:00")]
            [TestCase("00:60:00")]
            [TestCase("00:00:60")]
            public void MustThrowArgumentOutOfRangeExceptionWhenTimeIsOutOfRange(string source)
            {
                LocalTimeConverter converter = null;
                try
                {
                    converter = new LocalTimeConverter();
                }
                catch
                {
                    Assert.Inconclusive();
                }
                
                Assert.That(() => converter.Convert(source, new LocalTime(), null), Throws.InstanceOf<ArgumentOutOfRangeException>());
            }

            [Test]
            [TestCase("foo")]
            [TestCase("00.00.00")]
            [TestCase(".00.00.00")]
            [TestCase("00:00:00:00")]
            public void MustThrowFormatExceptionWhenInputIsInWrongFormat(string source)
            {
                LocalTimeConverter converter = null;
                try
                {
                    converter = new LocalTimeConverter();
                }
                catch
                {
                    Assert.Inconclusive();
                }

                Assert.That(() => converter.Convert(source, new LocalTime(), null), Throws.InstanceOf<FormatException>());
            }
        }
    }
}