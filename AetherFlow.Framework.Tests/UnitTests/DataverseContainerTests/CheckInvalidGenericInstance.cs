using System;
using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using AetherFlow.Framework.Tests.Setup.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests.DataverseContainerTests
{
    public class CheckInvalidGenericInstance : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseAssembly(GetType().Assembly);
            this.LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces");
            RunSpecification();
        }

        private IConverter<double, Guid> _example;

        public override void Act()
        {
            var container = this.GetContainer();
            _example = container.Get<IConverter<double, Guid>>();
        }

        [Test]
        public void EnsureExceptionThrown()
        {
            Assert.That(ThrownException, Is.Not.Null);
        }
    }
}