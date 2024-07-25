using AetherFlow.FakeXrmEasy.Plugins;
using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using AetherFlow.Framework.Tests.Setup.Models;
using AetherFlow.Framework.Tests.Setup.Plugins;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;
using System;

namespace AetherFlow.Framework.Tests.FeatureTests.PluginTests
{
    public class ThrowsCorrectException : SpecificationBase
    {
        private XrmFakedPluginExecutionContext _context;
        private Entity _target;

        [OneTimeSetUp]
        public void Run()
        {
            this.UseFakeXrmEasy();
            RunSpecification();
        }

        public override void Arrange()
        {
            _target = new Entity(Contact.LogicalName, Guid.NewGuid());

            _context = this.GetXrmFakedContext().GetDefaultPluginContext();
            _context.MessageName = "Update";
            _context.InputParameters = new ParameterCollection {
                { "Target", _target }
            };
        }

        public override void Act()
        {
            var context = this.GetXrmFakedContext();
            context.ExecutePluginWith<UpdateTargetContact>(
                _context
            );
        }

        [Test]
        public void EnsureCorrectExceptionThrown()
        {
            Assert.That(ThrownException, Is.TypeOf<InvalidPluginExecutionException>());
        }
    }
}
