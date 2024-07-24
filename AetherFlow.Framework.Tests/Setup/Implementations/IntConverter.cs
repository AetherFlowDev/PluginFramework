using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    public class IntConverter : IConverter<int, string>
    {
        public string From(int input) { return input.ToString(); }
        public int To(string input) { return 1; }
    }
}
