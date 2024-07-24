using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    public class MockExampleIntegration : IExampleIntegration
    {
        public bool DoAction() => true;
    }
}
