using AetherFlow.Framework.Attributes;
using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    [Variant("Other")]
    public class ExampleIntegrationVariant : IExampleIntegration
    {
        public bool DoAction() => true;
    }
}
