using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    public class BoolConverter : IConverter<bool, string>
    {
        public string From(bool input) { return input.ToString(); }
        public bool To(string input) { return true; }
    }
}
