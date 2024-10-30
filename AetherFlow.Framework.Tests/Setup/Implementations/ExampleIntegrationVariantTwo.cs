using AetherFlow.Framework.Attributes;
using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    [Variant("Two")]
    public class ExampleIntegrationVariantTwo : IExampleIntegration
    {
        public bool DoAction() => true;
    }
}
