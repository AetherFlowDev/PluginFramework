using System;
using AetherFlow.FakeXrmEasy.Plugins;
using AetherFlow.Framework.Helpers;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;
using AetherFlow.Framework.Tests.Setup.Models;
using AetherFlow.Framework.Tests.Setup.Actions.Config;
using AetherFlow.Framework.Tests.Setup.Plugins;
using AetherFlow.Framework.Tests.Support.SpecificationExtensions;

namespace AetherFlow.Framework.Tests.FeatureTests.PluginTests
{
    [TestFixture]
    public class UpdateTargetContactExecutes : SpecificationBase
    {
        private XrmFakedPluginExecutionContext _context;
        private Entity _target;
        private ContactPluginConfig _config;

        [OneTimeSetUp]
        public void Run() {
            this.UseFakeXrmEasy();
            RunSpecification(); 
        }

        public override void Arrange()
        {
            _target = new Entity(Contact.LogicalName, Guid.NewGuid());
            _config = new ContactPluginConfig { FirstName = "Test Name" };

            _context = this.GetXrmFakedContext().GetDefaultPluginContext();
            _context.MessageName = "Create";
            _context.InputParameters = new ParameterCollection {
                { "Target", _target }
            };
        }

        public override void Act()
        {
            var context = this.GetXrmFakedContext();
            context.ExecutePluginWithConfigurations<UpdateTargetContact>(
                _context,
                "",
                new JsonContractSerializer().Serialize(_config)
            );
        }

        [Test]
        public void EnsureNoExceptionThrown()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void EnsureContactIsUpdated()
        {
            Assert.That(_target.GetAttributeValue<string>("firstname"), Is.EqualTo(_config.FirstName));
        }
    }
}
