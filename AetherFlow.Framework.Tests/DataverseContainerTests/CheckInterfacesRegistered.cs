using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.DataverseContainerTests
{
    public class CheckInterfacesRegistered : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run() { RunSpecification(); }

        // ARRANGE variables
        private IDataverseContainer _container;

        public override void Arrange()
        {
            _container = new DataverseContainer();
        }

        public override void Act()
        {
            _container.Initialize(
                GetType().Assembly, 
                "AetherFlow.Framework.Tests.Example.Interfaces"
            );
        }

        [Test]
        public void EnsureNoExceptionThrown()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void EnsureContainerIsNotNull()
        {
            Assert.That(_container, Is.Not.Null);
        }
    }
}
