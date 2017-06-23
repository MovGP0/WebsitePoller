using System;
using System.IO;
using NodaTime;
using NUnit.Framework;

namespace WebsitePoller.Tests.Setting
{
    [TestFixture]
    public sealed class SettingsLoaderTests
    {
        public sealed class LoadTests
        {
            private static IAn An { get; }
            
            [Test]
            public void LoadSettingsFromFile()
            {
                var filePath = Path.Combine(An.AssemblyPath(), "settings.hjson");
                var settingsLoader = An.SettingsLoader();
                var settings = settingsLoader.Load(filePath);

                Assert.Multiple(() =>
                {
                    var s = settings;
                    Assert.That(s.From, Is.EqualTo(new LocalTime(21, 00)), nameof(s.From));
                    Assert.That(s.Till, Is.EqualTo(new LocalTime(23, 59)), nameof(s.Till));
                    Assert.That(s.Url, Is.EqualTo(new Uri("http://www.sozialbau.at/nc/home/suche/altbau-wohnungen/")), nameof(s.Url));
                    Assert.That(s.PostalCodes.Length, Is.EqualTo(1297), nameof(s.PostalCodes));
                    Assert.That(s.Cities.Length, Is.EqualTo(1), nameof(s.Cities));
                    Assert.That(s.PollingIntervallInSeconds, Is.EqualTo(20), nameof(s.PollingIntervallInSeconds));
                    Assert.That(s.MaxEigenmittel, Is.EqualTo(22000.00m), nameof(s.MaxEigenmittel));
                    Assert.That(s.MaxMonatlicheKosten, Is.EqualTo(850.00m), nameof(s.MaxMonatlicheKosten));
                    Assert.That(s.MinNumberOfRooms, Is.EqualTo(3), nameof(s.MinNumberOfRooms));

                    var pa = s.PostalAddress;
                    Assert.That(pa, Is.Not.Null, nameof(pa.PostalCode));
                    Assert.Multiple(() =>
                    {
                        Assert.That(pa?.ApartmentNumber, Is.EqualTo("8"), nameof(pa.ApartmentNumber));
                        Assert.That(pa?.BirthDate, Is.EqualTo(new DateTime(1989,11,14)), nameof(pa.BirthDate));
                        Assert.That(pa?.City, Is.EqualTo("Wien"), nameof(pa.City));
                        Assert.That(pa?.EmailAddress, Is.EqualTo("michael.stanojevic@gmx.at"), nameof(pa.EmailAddress));
                        Assert.That(pa?.FamilyName, Is.EqualTo("Stanojevic"), nameof(pa.FamilyName));
                        Assert.That(pa?.FirstName, Is.EqualTo("Michael"), nameof(pa.FirstName));
                        Assert.That(pa?.HouseNumber, Is.EqualTo("242"), nameof(pa.HouseNumber));
                        Assert.That(pa?.IsTenant, Is.EqualTo(false), nameof(pa.IsTenant));
                        Assert.That(pa?.PhoneNumber, Is.EqualTo("0676/7504304"), nameof(pa.PhoneNumber));
                        Assert.That(pa?.PostalCode, Is.EqualTo(1160), nameof(pa.PostalCode));
                        Assert.That(pa?.Salutation, Is.EqualTo("Herr"), nameof(pa.Salutation));
                        Assert.That(pa?.StairsNumber, Is.EqualTo("5"), nameof(pa.StairsNumber));
                        Assert.That(pa?.Street, Is.EqualTo("Ottakringer Straße"), nameof(pa.Street));
                        Assert.That(pa?.Title, Is.EqualTo(""), nameof(pa.Title));
                    });
                });
            }
        }
    }
}
