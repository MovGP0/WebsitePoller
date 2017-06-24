using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using DryIoc;
using JetBrains.Annotations;
using NodaTime;
using WebsitePoller.Entities;
using WebsitePoller.Mappings;
using WebsitePoller.Parser;
using WebsitePoller.Setting;

namespace WebsitePoller.Tests
{
    public static class AnExtensions
    {
        [NotNull]
        public static string AssemblyPath(this IAn an)
        {
            var result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (result == null) throw new Exception("Assembly path was null.");
            return result;
        }

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
                Street = "Straﬂe", 
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

        private static readonly Lazy<SettingsManager> SettingsManagerFactory 
            = new Lazy<SettingsManager>(() => new SettingsManager());

        public static SettingsManager SettingsManager(this IAn an)
        {
            return SettingsManagerFactory.Value;
        }

        public static SettingsLoader SettingsLoader(this IAn an)
        {
            var settingsManager = an.SettingsManager();
            var mapper = an.Mapper();
            return new SettingsLoader(settingsManager, mapper);
        }

        public static IMapper Mapper(this IAn an)
        {
            var resolver = an.Resolver();
            return resolver.Resolve<IMapper>();
        }

        public static AltbauWohnungInfo AltbauWohnungInfo(this IAn an)
        {
            return new AltbauWohnungInfo
            {
                City = "Wien",
                PostalCode = 1010,
                Eigenmittel = 20000,
                MonatlicheKosten = 700,
                Street = "Straﬂe",
                Href = "/foo%20",
                NumberOfRooms = 3
            };
        }

        public static AltbauWohnungInfoEqualityComparer AltbauWohnungInfoEqualityComparer(this IAn an)
        {
            return new AltbauWohnungInfoEqualityComparer();
        }

        public static VersionFromTillEqualityComparer VersionFromTillEqualityComparer(this IAn an)
        {
            return new VersionFromTillEqualityComparer();
        }

        public static AddressFieldParser AddressFieldParser(this IAn an)
        {
            return new AddressFieldParser();
        }
    }
}