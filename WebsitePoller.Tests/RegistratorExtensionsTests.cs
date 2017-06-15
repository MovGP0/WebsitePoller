using System;
using DryIoc;
using NodaTime;
using NUnit.Framework;

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
