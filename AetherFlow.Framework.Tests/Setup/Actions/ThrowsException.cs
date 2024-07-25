using System;
using AetherFlow.Framework.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Actions
{
    public class ThrowsException : IPluginAction
    {
        public ThrowsException()
        {
            
        }

        public void Execute()
        {
            throw new Exception("This is a test");
        }
    }
}
