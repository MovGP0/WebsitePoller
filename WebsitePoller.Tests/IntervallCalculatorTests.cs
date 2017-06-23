using NodaTime;
using NodaTime.Testing;
using NSubstitute;
using NUnit.Framework;
using WebsitePoller.Entities;

namespace WebsitePoller.Tests
{
    [TestFixture]
    public sealed class IntervallCalculatorTests
    {
        public sealed class CalculateDurationTillIntervall
        {
            private static IAn An { get; }

            [Test]
            public void ConstructorShouldNotThrow()
            {
                var clock = Substitute.For<IClock>();
                var settingsManager = An.SettingsManager();

                Assert.That(() => new IntervallCalculator(clock, settingsManager), Throws.Nothing);
            }

            [Test]
            public void ShouldCalculateIntervall()
            {
                var settings = new Settings
                {
                    TimeZone = "Europe/Vienna", 
                    From = new LocalTime(21,00), 
                    Till = new LocalTime(23,00)
                };
                
                var dateTimeZone = DateTimeZoneProviders.Tzdb[settings.TimeZone];
                var time = new LocalDateTime(2017, 06, 23, 20, 00);
                var zonedDateTime = dateTimeZone.AtLeniently(time);
                var instant = zonedDateTime.ToInstant();

                var clock = new FakeClock(instant);
                
                var settingsManager = An.SettingsManager();
                settingsManager.Settings = settings;

                var calculator = new IntervallCalculator(clock, settingsManager);
                var intervall = calculator.CalculateDurationTillIntervall();
                Assert.That(intervall, Is.EqualTo(Duration.FromHours(1)));
            }
        }
    }
}
