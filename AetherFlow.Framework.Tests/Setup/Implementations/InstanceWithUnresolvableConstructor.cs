using System;
using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    public class InstanceWithUnresolvableConstructor : IInstanceWithUnresolvableConstructor
    {
        public InstanceWithUnresolvableConstructor(IServiceProvider provider)
        {

        }

        public bool DoAction() => true;
    }
}
