using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework.Tests.Setup.Implementations;
using AetherFlow.Framework.Tests.Setup.Interfaces;
using AetherFlow.Framework.Tests.Support.SpecificationExtensions;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests.DataverseContainerTests
{
    public class CheckNoConstructorWorks : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseAssembly(GetType().Assembly);
            this.LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces");
            RunSpecification();
        }

        // ACT variables
        private IInstanceWithNoConstructor _instance;

        public override void Act()
        {
            var container = this.GetContainer();
            _instance = container.Get<IInstanceWithNoConstructor>();
        }

        [Test]
        public void EnsureNoExceptionThrown()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void EnsureInstanceIsNotNull()
        {
            Assert.That(_instance, Is.Not.Null);
        }

        [Test]
        public void EnsureInstanceActionCanBeCalled()
        {
            Assert.That(_instance.DoAction(), Is.True);
        }

        [Test]
        public void EnsureInstanceOfCorrectType()
        {
            Assert.That(_instance, Is.TypeOf<InstanceWithNoConstructor>());
        }
    }
}
