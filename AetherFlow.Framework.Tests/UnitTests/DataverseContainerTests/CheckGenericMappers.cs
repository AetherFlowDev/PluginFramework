using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework.Tests.Setup.Implementations;
using AetherFlow.Framework.Tests.Setup.Interfaces;
using AetherFlow.Framework.Tests.Setup.Models;
using AetherFlow.Framework.Tests.Support.SpecificationExtensions;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests.DataverseContainerTests
{
    public class CheckGenericMappers : SpecificationBase
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
        private IMapper<Contact> _contactInstance;
        private IMapper<InstanceWithNoConstructor> _exampleInstance;
        private IConverter<bool, string> _boolConverter;
        private IConverter<int, string> _intConverter;

        public override void Act()
        {
            var container = this.GetContainer();
            _contactInstance = container.Get<IMapper<Contact>>();
            _exampleInstance = container.Get<IMapper<InstanceWithNoConstructor>>();
            _boolConverter = container.Get<IConverter<bool, string>>();
            _intConverter = container.Get<IConverter<int, string>>();
        }

        [Test]
        public void EnsureNoException()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void EnsureCorrectTypes()
        {
            Assert.That(_contactInstance, Is.InstanceOf<ContactMapper>());
            Assert.That(_exampleInstance, Is.InstanceOf<ExampleMapper>());
            Assert.That(_boolConverter, Is.InstanceOf<BoolConverter>());
            Assert.That(_intConverter, Is.InstanceOf<IntConverter>());
        }
    }
}
