using AetherFlow.Framework.Tests.Example.Interfaces;

namespace AetherFlow.Framework.Tests.Example.Implementations
{
    public class IntConverter : IConverter<int, string>
    {
        public string From(int input) { return input.ToString(); }
        public int To(string input) { return 1; }
    }
}
