﻿using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using AetherFlow.Framework.Tests.Setup.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests.DataverseContainerTests
{
    public class CheckMockInstances : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseAssembly(GetType().Assembly);
            this.LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces");
            RunSpecification();
        }

        private IExampleIntegration _example;

        public override void Act()
        {
            var container = this.GetContainer();
            _example = container.Get<IExampleIntegration>();
        }

        [Test]
        public void EnsureExceptionThrown()
        {
            Assert.That(ThrownException, Is.Not.Null);
        }
    }
}