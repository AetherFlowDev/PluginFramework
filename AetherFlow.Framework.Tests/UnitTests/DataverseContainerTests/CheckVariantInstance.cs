using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using AetherFlow.Framework.Tests.Setup.Implementations;
using AetherFlow.Framework.Tests.Setup.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests.DataverseContainerTests
{
    public class CheckVariantInstance : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseAssembly(GetType().Assembly);
            this.LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces");
            RunSpecification();
        }

        private IExampleIntegration _exampleOne;
        private IExampleIntegration _exampleTwo;

        public override void Act()
        {
            var container = this.GetContainer();
            
            container.UseVariant<IExampleIntegration>("Two");
            _exampleOne = container.Get<IExampleIntegration>();

            container.UseVariant<IExampleIntegration>("Other");
            _exampleTwo = container.Get<IExampleIntegration>();
        }

        [Test]
        public void EnsureCorrectImplementations()
        {
            Assert.That(_exampleOne, Is.InstanceOf<ExampleIntegrationVariantTwo>());
            Assert.That(_exampleTwo, Is.InstanceOf<ExampleIntegrationVariant>());
        }
    }
}