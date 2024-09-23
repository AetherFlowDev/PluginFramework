using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using AetherFlow.Framework.Tests.Setup.Implementations;
using AetherFlow.Framework.Tests.Setup.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests.DataverseContainerTests
{
    [TestFixture]
    public class CheckDuplicatesRemovedWithoutError : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            RunSpecification();
        }

        private IDataverseContainer _container;

        public override void Arrange()
        {
            _container = this.GetContainer();
            _container.Add<IInvoice>(new Invoice());
        }

        public override void Act()
        {
            _container.Initialize(GetType().Assembly, "AetherFlow.Framework.Tests.Setup.Interfaces");
        }

        [Test]
        public void EnsureNoErrors()
        {
            Assert.That(ThrownException, Is.Null);
        }
    }
}
