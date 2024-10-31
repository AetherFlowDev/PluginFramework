using AetherFlow.Framework.Attributes;
using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    [Mock]
    public class MainInstanceMock : IMainInstance
    {
        public int DoAction() => 2;
    }
}
