using AetherFlow.FakeXrmEasy.Plugins;
using AetherFlow.Framework.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests
{
    public class PluginSpecification : SpecificationBase
    {
        protected IDataverseContainer Container;
        protected XrmFakedContext Context;

        [OneTimeSetUp]
        public void SetupCrmService()
        {
            Context = new XrmFakedContext();
        }
    }
}
