using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    public class InstanceWithTwoConstructors : IInstanceWithTwoConstructors
    {
        private readonly ICoreInstanceOne _core;

        public InstanceWithTwoConstructors(ICoreInstanceOne coreInstanceOne)
        {
            _core = coreInstanceOne;
        }

        public InstanceWithTwoConstructors()
        {
        }

        public bool DoAction() => _core.DoAction();
    }
}
