using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    public class ExampleIntegrationMock : IExampleIntegration
    {
        public bool DoAction() => true;
    }
}
