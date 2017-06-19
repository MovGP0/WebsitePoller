using System;
using DryIoc;
using NodaTime;
using NUnit.Framework;
using WebsitePoller.Workflow;

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
        [TestCase(typeof(IPolicyFactory))]
        [TestCase(typeof(IIntervallCalculator))]
        [TestCase(typeof(ITownCrier))]
        [TestCase(typeof(IClock))]
        [TestCase(typeof(ITownCrierFactory))]
        [TestCase(typeof(ISettingsLoader))]
        [TestCase(typeof(Settings))]
        [TestCase(typeof(IExecuteWorkFlowCommand))]
        [TestCase(typeof(IFileContentComparer))]
        [TestCase(typeof(INotifier))]
        public void MustHaveInterfaceRegistered(Type interfaceType)
        {
            IRegistrator registrator;
            try
            {
                var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());
                container.SetupDependencies();
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
        public void MustBeRegisteredAs(Type interfaceType, Type instanceType)
        {
            IResolver resolver;

            try
            {
                var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());
                container.SetupDependencies();
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
