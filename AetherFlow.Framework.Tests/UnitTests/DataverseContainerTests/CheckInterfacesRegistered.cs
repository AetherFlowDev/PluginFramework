using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests.DataverseContainerTests
{
    public class CheckInterfacesRegistered : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseAssembly(GetType().Assembly);
            this.LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces");
            RunSpecification();
        }

        [Test]
        public void EnsureNoExceptionThrown()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void EnsureContainerIsNotNull()
        {
            var container = this.GetContainer();
            Assert.That(container, Is.Not.Null);
        }
    }
}
