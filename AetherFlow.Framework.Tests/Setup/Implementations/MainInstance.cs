using AetherFlow.Framework.Attributes;
using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    [Main]
    public class MainInstance : IMainInstance
    {
        public int DoAction() => 1;
    }
}
