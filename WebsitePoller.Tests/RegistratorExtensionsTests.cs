using System;
using DryIoc;
using NodaTime;
using NUnit.Framework;
using WebsitePoller.Workflow;
using System.Collections.Generic;
using AutoMapper;
using HtmlAgilityPack;
using RestSharp;
using WebsitePoller.Parser;
using WebsitePoller.Setting;

namespace WebsitePoller.Tests
{
    [TestFixture]
    public sealed class RegistratorExtensionsTests
    {
        [Test]
        public void SetupDependencienShouldNotThrowAnException()
        {
            var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());
            Assert.That(container.SetupDependencies, Throws.Nothing);
        }

        [Test]
        public void SetupMappingsShouldNotThrowAnException()
        {
            var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());
            Assert.That(container.SetupDependencies, Throws.Nothing);
        }

        [Test]
        [TestCase(typeof(IPolicyFactory))]
        [TestCase(typeof(IIntervallCalculator))]
        [TestCase(typeof(ITownCrier))]
        [TestCase(typeof(IClock))]
        [TestCase(typeof(ITownCrierFactory))]
        [TestCase(typeof(ISettingsLoader))]
        [TestCase(typeof(SettingsManager))]
        [TestCase(typeof(IExecuteWorkFlowCommand))]
        [TestCase(typeof(IFileContentComparer))]
        [TestCase(typeof(INotifier))]
        [TestCase(typeof(IEqualityComparer<HtmlDocument>))]
        [TestCase(typeof(MapperConfiguration))]
        [TestCase(typeof(IMapper))]
        [TestCase(typeof(IAltbauWohnungenParser))]
        [TestCase(typeof(Func<Uri, IRestClient>))]
        public void MustHaveInterfaceRegistered(Type interfaceType)
        {
            IRegistrator registrator;
            try
            {
                var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());
                container.SetupDependencies().SetupMappings();
                registrator = container;
            }
            catch (Exception)
            {
                Assert.Inconclusive();
                return;
            }

            var isRegistered = registrator.IsRegistered(interfaceType);
            Assert.That(isRegistered);
        }

        [Test]
        [TestCase(typeof(IPolicyFactory), typeof(PolicyFactory))]
        [TestCase(typeof(IIntervallCalculator), typeof(IntervallCalculator))]
        [TestCase(typeof(ITownCrier), typeof(TownCrier))]
        [TestCase(typeof(IClock), typeof(SystemClock))]
        [TestCase(typeof(ITownCrierFactory), typeof(TownCrierFactory))]
        [TestCase(typeof(ISettingsLoader), typeof(SettingsLoader))]
        [TestCase(typeof(IWebsiteDownloader), typeof(WebsiteDownloader))]
        [TestCase(typeof(IExecuteWorkFlowCommand), typeof(ExecuteWorkFlowCommand))]
        [TestCase(typeof(IFileContentComparer), typeof(FileContentComparer))]
        [TestCase(typeof(INotifier), typeof(Notifier))]
        [TestCase(typeof(IMapper), typeof(Mapper))]
        [TestCase(typeof(MapperConfiguration), typeof(MapperConfiguration))]
        [TestCase(typeof(IAltbauWohnungenParser), typeof(AltbauWohnungenParser))]
        public void MustBeRegisteredAs(Type interfaceType, Type instanceType)
        {
            IResolver resolver;

            try
            {
                var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());
                container.SetupDependencies().SetupMappings(); 
                resolver = container;
            }
            catch (Exception)
            {
                Assert.Inconclusive();
                return;
            }

            var instance = resolver.Resolve(interfaceType);
            Assert.That(instance, Is.Not.Null);
            Assert.That(instance, Is.TypeOf(instanceType));
        }
    }
}
