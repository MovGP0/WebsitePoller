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
            [TestCase(00, 00, 21, 00)]
            [TestCase(20, 00, 01, 00)]
            [TestCase(20, 59, 00, 01)]
            [TestCase(21, 00, 00, 00)]
            [TestCase(22, 00, 00, 00)]
            [TestCase(23, 00, 00, 00)]
            [TestCase(23, 01, 21, 59)]
            public void ShouldCalculateIntervall(int currentHour, int currentMinute, int hours, int minutes)
            {
                var expected = Duration.FromHours(hours) + Duration.FromMinutes(minutes);
                var settings = new Settings
                {
                    TimeZone = "Europe/Vienna", 
                    From = new LocalTime(21,00), 
                    Till = new LocalTime(23,00)
                };
                var time = new LocalDateTime(2017, 06, 23, currentHour, currentMinute);
                var clock = CreateFakeClock(settings, time);

                var settingsManager = An.SettingsManager();
                settingsManager.Settings = settings;

                var calculator = new IntervallCalculator(clock, settingsManager);
                var intervall = calculator.CalculateDurationTillIntervall();
                Assert.That(intervall, Is.EqualTo(expected));
            }
            
            [Test]
            [TestCase(23, 00, 00, 00)]
            [TestCase(00, 01, 00, 00)]
            [TestCase(01, 59, 00, 00)]
            public void ShouldCalculateIntervallWhenTillIsAfterMidnignt(int currentHour, int currentMinute, int hours, int minutes)
            {
                var expected = Duration.FromHours(hours) + Duration.FromMinutes(minutes);
                var settings = new Settings
                {
                    TimeZone = "Europe/Vienna",
                    From = new LocalTime(23, 00),
                    Till = new LocalTime(02, 00)
                };
                var time = new LocalDateTime(2017, 06, 23, currentHour, currentMinute);
                var clock = CreateFakeClock(settings, time);

                var settingsManager = An.SettingsManager();
                settingsManager.Settings = settings;

                var calculator = new IntervallCalculator(clock, settingsManager);
                var intervall = calculator.CalculateDurationTillIntervall();
                Assert.That(intervall, Is.EqualTo(expected));
            }

            [Test]
            public void ShouldCalculateIntervallAtSummerToWinterChange()
            {
                var expected = Duration.FromHours(22) + Duration.FromMinutes(30);
                var settings = new Settings
                {
                    TimeZone = "Europe/Vienna",
                    From = new LocalTime(21, 00),
                    Till = new LocalTime(23, 00)
                };
                var time = new LocalDateTime(2017, 10, 28, 23, 30);
                var clock = CreateFakeClock(settings, time);

                var settingsManager = An.SettingsManager();
                settingsManager.Settings = settings;

                var calculator = new IntervallCalculator(clock, settingsManager);
                var intervall = calculator.CalculateDurationTillIntervall();
                Assert.That(intervall, Is.EqualTo(expected));
            }

            [Test]
            public void ShouldCalculateIntervallAtWinterToSummerChange()
            {
                var expected = Duration.FromHours(20) + Duration.FromMinutes(30);
                var settings = new Settings
                {
                    TimeZone = "Europe/Vienna",
                    From = new LocalTime(21, 00),
                    Till = new LocalTime(23, 00)
                };
                var time = new LocalDateTime(2017, 03, 25, 23, 30);
                var clock = CreateFakeClock(settings, time);

                var settingsManager = An.SettingsManager();
                settingsManager.Settings = settings;

                var calculator = new IntervallCalculator(clock, settingsManager);
                var intervall = calculator.CalculateDurationTillIntervall();
                Assert.That(intervall, Is.EqualTo(expected));
            }

            private static IClock CreateFakeClock(SettingsBase settings, LocalDateTime time)
            {
                var dateTimeZone = DateTimeZoneProviders.Tzdb[settings.TimeZone];
                var zonedDateTime = dateTimeZone.AtLeniently(time);
                var instant = zonedDateTime.ToInstant();
                return new FakeClock(instant);
            }
        }
    }
}
