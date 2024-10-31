using System;
using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using AetherFlow.Framework.Tests.Setup.Implementations;
using AetherFlow.Framework.Tests.Setup.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests.DataverseContainerTests
{
    public class CheckGenericMockInstances : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseAssembly(GetType().Assembly);
            this.LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces");
            RunSpecification();
        }

        private IConverter<Guid, string> _example;

        public override void Act()
        {
            var container = this.GetContainer();
            container.UseMock<IConverter<Guid, string>>();
            _example = container.Get<IConverter<Guid, string>>();
        }

        [Test]
        public void EnsureExceptionIsNotThrown()
        {
            Assert.That(ThrownException, Is.Null);
        }
        
        [Test]
        public void EnsureMockIsRetrieved()
        {
            Assert.That(_example, Is.InstanceOf<GuidConverterMock>());
        }
    }
}