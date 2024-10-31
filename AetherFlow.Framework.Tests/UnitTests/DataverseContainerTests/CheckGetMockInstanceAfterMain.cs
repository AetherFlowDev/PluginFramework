using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using AetherFlow.Framework.Tests.Setup.Implementations;
using AetherFlow.Framework.Tests.Setup.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests.DataverseContainerTests
{
    public class CheckGetMockInstanceAfterMain : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseAssembly(GetType().Assembly);
            this.LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces");
            RunSpecification();
        }

        private IMainInstance _one;
        private IMainInstance _two;

        public override void Act()
        {
            var container = this.GetContainer();
            _one = container.Get<IMainInstance>();
            container.UseMock<IMainInstance>();
            _two = container.Get<IMainInstance>();
        }

        [Test]
        public void EnsureNoExceptions()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void EnsureCorrectInstances()
        {
            Assert.That(_one, Is.InstanceOf<MainInstance>());
            Assert.That(_two, Is.InstanceOf<MainInstanceMock>());
        }
    }
}