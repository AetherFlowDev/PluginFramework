using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework.Tests.Content.Models;
using AetherFlow.Framework.Tests.Example.Implementations;
using AetherFlow.Framework.Tests.Example.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.DataverseContainerTests
{
    public class CheckGenericMappers : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run() { RunSpecification(); }

        // ARRANGE variables
        private IDataverseContainer _container;

        // ACT variables
        private IMapper<Contact> _contactInstance;
        private IMapper<InstanceWithNoConstructor> _exampleInstance;
        private IConverter<bool, string> _boolConverter;
        private IConverter<int, string> _intConverter;

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
            _contactInstance = _container.Get<IMapper<Contact>>();
            _exampleInstance = _container.Get<IMapper<InstanceWithNoConstructor>>();
            _boolConverter = _container.Get<IConverter<bool, string>>();
            _intConverter = _container.Get<IConverter<int, string>>();
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
