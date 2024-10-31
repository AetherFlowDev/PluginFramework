using AetherFlow.Framework.Tests.Setup.Interfaces;
using System;
using AetherFlow.Framework.Attributes;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    [Mock]
    public class GuidConverterMock : IConverter<Guid, string>
    {
        public string From(Guid input) { return input.ToString(); }
        public Guid To(string input) { return Guid.NewGuid(); }
    }
}
