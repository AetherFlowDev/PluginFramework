using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework.Tests.Example.Implementations;
using AetherFlow.Framework.Tests.Example.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.DataverseContainerTests
{
    public class CheckNoConstructorWorks : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run() { RunSpecification(); }

        // ARRANGE variables
        private IDataverseContainer _container;
        private IInstanceWithNoConstructor _instance;

        public override void Arrange()
        {
            _container = new DataverseContainer();
            _container.Initialize(
                GetType().Assembly,
                "AetherFlow.Framework.Tests.Example.Interfaces"
            );
        }

        public override void Act()
        {
            _instance = _container.Get<IInstanceWithNoConstructor>();
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
