﻿using AetherFlow.Framework.Attributes;
using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    [Mock]
    public class ExampleIntegrationMock : IExampleIntegration
    {
        public bool DoAction() => true;
    }
}
