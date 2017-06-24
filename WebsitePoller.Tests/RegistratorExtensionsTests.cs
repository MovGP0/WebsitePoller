using System;
using DryIoc;
using NodaTime;
using NUnit.Framework;
using WebsitePoller.Workflow;
using System.Collections.Generic;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
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
        private IAn An { get; }
        
        [Test]
        public void SetupContainerShouldNotThrowAnException()
        {
            Assert.That(() => An.Container(), Throws.Nothing);
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
        [TestCase(typeof(IToastNotifier))]
        [TestCase(typeof(ToastNotifier))]
        [TestCase(typeof(IAddressFieldParser))]
        [TestCase(typeof(IAltbauWohnungenFilter))]
        [TestCase(typeof(IAltbauWohnungenRowParser))]
        [TestCase(typeof(Func<XmlDocument, ToastNotification>))]
        public void MustHaveInterfaceRegistered(Type interfaceType)
        {
            IRegistrator registrator;
            try
            {
                registrator = An.Registrator();
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
        [TestCase(typeof(IToastNotifier), typeof(ToastNotifierWrapper))]
        [TestCase(typeof(ToastNotifier), typeof(ToastNotifier))]
        [TestCase(typeof(IAddressFieldParser), typeof(AddressFieldParser))]
        [TestCase(typeof(IAltbauWohnungenFilter), typeof(AltbauWohnungenFilter))]
        [TestCase(typeof(IAltbauWohnungenRowParser), typeof(AltbauWohnungenRowParser))]
        public void MustBeRegisteredAs(Type interfaceType, Type instanceType)
        {
            IResolver resolver;

            try
            {
                resolver = An.Resolver();
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
