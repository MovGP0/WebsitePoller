using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using DryIoc;
using HtmlAgilityPack;
using JetBrains.Annotations;
using NodaTime;
using WebsitePoller.Entities;
using WebsitePoller.Mappings;
using WebsitePoller.Parser;
using WebsitePoller.Setting;
using WebsitePoller.Workflow;

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

        [NotNull]
        public static SettingsBaseEqualityComparer SettingsBaseEqualityComparer(this IAn an)
        {
            return new SettingsBaseEqualityComparer();
        }

        [NotNull]
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
                PostalCodes = new[] { 1000, 1010 },
                MinNumberOfRooms = 3,
                PollingIntervallInSeconds = 20
            };
        }

        [NotNull]
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
                PostalCodes = new []{ 1000, 1010 }, 
                MinNumberOfRooms = 3, 
                PollingIntervallInSeconds = 20
            };
        }

        [NotNull]
        public static AltbauWohnungInfo AltbauWohnungInfo2(this IAn an)
        {
            return new AltbauWohnungInfo
            {
                City = "Wien",
                PostalCode = 1010,
                Eigenmittel = 10000m,
                MonatlicheKosten = 800m,
                Street = "Straße",
                Href = "/foo%20",
                NumberOfRooms = 3
            };
        }

        [NotNull]
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

        [NotNull]
        public static IResolver Resolver(this IAn an)
        {
            return an.Container();
        }

        [NotNull]
        public static IRegistrator Registrator(this IAn an)
        {
            return an.Container();
        }

        [NotNull]
        public static Container Container(this IAn an)
        {
            var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());
            container
                .SetupMappings()
                .SetupDependencies();
            return container;
        }

        [NotNull]
        private static readonly Lazy<SettingsManager> SettingsManagerFactory 
            = new Lazy<SettingsManager>(() => new SettingsManager());

        [NotNull]
        public static SettingsManager SettingsManager(this IAn an)
        {
            return SettingsManagerFactory.Value;
        }

        [NotNull]
        public static SettingsLoader SettingsLoader(this IAn an)
        {
            var settingsManager = an.SettingsManager();
            var mapper = an.Mapper();
            return new SettingsLoader(settingsManager, mapper);
        }

        [NotNull]
        public static IMapper Mapper(this IAn an)
        {
            var resolver = an.Resolver();
            return resolver.Resolve<IMapper>();
        }

        [NotNull]
        public static AltbauWohnungInfo AltbauWohnungInfo(this IAn an)
        {
            return new AltbauWohnungInfo
            {
                City = "Wien",
                PostalCode = 1010,
                Eigenmittel = 20000,
                MonatlicheKosten = 700,
                Street = "Straße",
                Href = "/foo%20",
                NumberOfRooms = 3
            };
        }

        [NotNull]
        public static AltbauWohnungInfoEqualityComparer AltbauWohnungInfoEqualityComparer(this IAn an)
        {
            return new AltbauWohnungInfoEqualityComparer();
        }

        [NotNull]
        public static VersionFromTillEqualityComparer VersionFromTillEqualityComparer(this IAn an)
        {
            return new VersionFromTillEqualityComparer();
        }

        [NotNull]
        public static AddressFieldParser AddressFieldParser(this IAn an)
        {
            return new AddressFieldParser();
        }

        [NotNull]
        public static AddressFieldParserResult AddressFieldParserResult(this IAn an)
        {
            return new AddressFieldParserResult
            {
                City = "Wien",
                PostalCode = 1010,
                Street = "Gürtel Straße 3-2a/16"
            };
        }

        [NotNull]
        public static HtmlDocument HtmlDocument(this IAn an)
        {
            var assemblyPath = an.AssemblyPath();
            var documentPath = Path.Combine(assemblyPath, "altbau-wohnungen.html");
            var document = new HtmlDocument();
            using (var stream = File.OpenRead(documentPath))
            {
                document.Load(stream);
            }

            return document;
        }

        [NotNull]
        public static HtmlDocument EmptyHtmlDocument(this IAn an)
        {
            var assemblyPath = an.AssemblyPath();
            var documentPath = Path.Combine(assemblyPath, "empty.html");
            var document = new HtmlDocument();
            using (var stream = File.OpenRead(documentPath))
            {
                document.Load(stream);
            }

            return document;
        }

        [NotNull]
        public static HtmlDocumentComparer HtmlDocumentComparer(this IAn an)
        {
            return new HtmlDocumentComparer();
        }
    }
}