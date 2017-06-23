using System;
using DryIoc;
using NodaTime;
using WebsitePoller.Entities;
using WebsitePoller.Mappings;

namespace WebsitePoller.Tests
{
    public static class AnExtensions
    {
        public static SettingsBaseEqualityComparer SettingsBaseEqualityComparer(this IAn an)
        {
            return new SettingsBaseEqualityComparer();
        }

        public static SettingsStrings SettingsStrings(this IAn an)
        {
            return new SettingsStrings
            {
                PostalAddress = an.PostalAddress(),
                From = "10:20:30",
                Till = "12:23:45",
                Cities = new[] { "Wien" },
                MaxEigenmittel = 10000m,
                Url = new Uri(@"/foo/bar", UriKind.Relative),
                MaxMonatlicheKosten = 800m,
                TimeZone = @"Europe/Vienna",
                PostalCodes = new[] { 1000 },
                MinNumberOfRooms = 3,
                PollingIntervallInSeconds = 20
            };
        }

        public static Settings Settings(this IAn an)
        {
            return new Settings
            {
                PostalAddress = an.PostalAddress(),
                From = new LocalTime(10, 20, 30),
                Till = new LocalTime(12, 23, 45),
                Cities = new[] {"Wien"}, 
                MaxEigenmittel = 10000m, 
                Url = new Uri(@"/foo/bar", UriKind.Relative), 
                MaxMonatlicheKosten = 800m, 
                TimeZone = @"Europe/Vienna", 
                PostalCodes = new []{ 1000 }, 
                MinNumberOfRooms = 3, 
                PollingIntervallInSeconds = 20
            };
        }

        public static PostalAddress PostalAddress(this IAn an)
        {
            return new PostalAddress
            {
                BirthDate = new DateTime(1980, 12, 31), 
                PhoneNumber = "0664/123 45 67",
                FirstName = "Franz", 
                FamilyName = "Mustermann", 
                IsTenant = false, 
                PostalCode = 2000, 
                Street = "Straße", 
                EmailAddress = "foo@bar.com", 
                HouseNumber = "12a", 
                Title = "", 
                Salutation = "Herr", 
                City = "Wien", 
                StairsNumber = "4b", 
                ApartmentNumber = "9c"
            };
        }
        
        public static IResolver Resolver(this IAn an)
        {
            return an.Container();
        }

        public static IRegistrator Registrator(this IAn an)
        {
            return an.Container();
        }

        public static Container Container(this IAn an)
        {
            var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());
            container
                .SetupMappings()
                .SetupDependencies();
            return container;
        }

    }
}