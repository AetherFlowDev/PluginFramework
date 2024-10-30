using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using AetherFlow.Framework.Tests.Setup.Implementations;
using AetherFlow.Framework.Tests.Setup.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests.DataverseContainerTests
{
    public class CheckGetMockInstance : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseAssembly(GetType().Assembly);
            this.LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces");
            RunSpecification();
        }

        private IExampleIntegration _example;
        
        public override void Act()
        {
            var container = this.GetContainer();
            container.UseMock<IExampleIntegration>();
            _example = container.Get<IExampleIntegration>();
        }

        [Test]
        public void EnsureWeHaveCorrectInstance()
        {
            Assert.That(ThrownException, Is.Null);
            Assert.That(_example, Is.InstanceOf<ExampleIntegrationMock>());
        }
    }
}