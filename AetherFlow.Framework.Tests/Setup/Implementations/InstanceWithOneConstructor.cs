using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    public class InstanceWithOneConstructor : IInstanceWithOneConstructor
    {
        private readonly ICoreInstanceOne _core;

        public InstanceWithOneConstructor(ICoreInstanceOne instance)
        {
            _core = instance;
        }

        public bool DoAction() => _core.DoAction();
    }
}
