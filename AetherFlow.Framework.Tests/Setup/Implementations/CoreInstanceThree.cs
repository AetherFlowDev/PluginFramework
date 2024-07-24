using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    public class CoreInstanceThree : ICoreInstanceThree
    {
        private readonly bool _actionResponse;

        // Several Invalid Constructors
        public CoreInstanceThree()
        {
            _actionResponse = false;
        }

        // Correct Constructor
        public CoreInstanceThree(ICoreInstanceOne one)
        {
            one.DoAction();
            _actionResponse = true;
        }

        public bool DoAction() => _actionResponse;
    }
}
