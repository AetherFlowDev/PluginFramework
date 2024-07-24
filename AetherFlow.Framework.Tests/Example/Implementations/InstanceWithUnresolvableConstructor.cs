using System;
using AetherFlow.Framework.Tests.Example.Interfaces;

namespace AetherFlow.Framework.Tests.Example.Implementations
{
    public class InstanceWithUnresolvableConstructor : IInstanceWithUnresolvableConstructor
    {
        public InstanceWithUnresolvableConstructor(IServiceProvider provider)
        {

        }

        public bool DoAction() => true;
    }
}
