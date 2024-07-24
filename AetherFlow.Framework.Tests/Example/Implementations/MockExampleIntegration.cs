using AetherFlow.Framework.Tests.Example.Interfaces;

namespace AetherFlow.Framework.Tests.Example.Implementations
{
    public class MockExampleIntegration : IExampleIntegration
    {
        public bool DoAction() => true;
    }
}
