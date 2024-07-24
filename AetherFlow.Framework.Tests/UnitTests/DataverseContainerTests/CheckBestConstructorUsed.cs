using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework.Tests.Setup.Interfaces;
using AetherFlow.Framework.Tests.Support.SpecificationExtensions;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests.DataverseContainerTests
{
    public class CheckBestConstructorUsed : SpecificationBase
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
        private ICoreInstanceThree _instance;

        public override void Act()
        {
            var container = this.GetContainer();
            _instance = container.Get<ICoreInstanceThree>();
        }

        [Test]
        public void EnsureNoExceptionThrown()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void EnsureActionReturnsTrue()
        {
            Assert.That(_instance.DoAction(), Is.True);
        }
    }
}