using System;
using AetherFlow.Framework.Attributes;
using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    [Main]
    public class GuidConverterMain : IConverter<Guid, string>
    {
        public string From(Guid input) { return input.ToString(); }
        public Guid To(string input) { return Guid.NewGuid(); }
    }
}
