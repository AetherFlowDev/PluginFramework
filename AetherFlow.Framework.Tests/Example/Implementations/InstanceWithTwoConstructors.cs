using AetherFlow.Framework.Tests.Example.Interfaces;

namespace AetherFlow.Framework.Tests.Example.Implementations
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
