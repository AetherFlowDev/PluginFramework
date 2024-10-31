using AetherFlow.Framework.Attributes;
using AetherFlow.Framework.Tests.Setup.Interfaces;
using System;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    [Variant("Other")]
    public class GuidConverterVariant : IConverter<Guid, string>
    {
        public string From(Guid input) { return input.ToString(); }
        public Guid To(string input) { return Guid.NewGuid(); }
    }
}
