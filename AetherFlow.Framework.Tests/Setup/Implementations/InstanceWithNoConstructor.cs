using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    public class InstanceWithNoConstructor : IInstanceWithNoConstructor
    {
        public bool DoAction() => true;
    }
}
