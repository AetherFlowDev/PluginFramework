using AetherFlow.Framework.Tests.Example.Interfaces;

namespace AetherFlow.Framework.Tests.Example.Implementations
{
    public class CoreInstanceThree : ICoreInstanceThree
    {
        private bool _actionResponse;

        // Several Invalid Constructors
        public CoreInstanceThree()
        {
            _actionResponse = false;
        }

        // Correct Constructor
        public CoreInstanceThree(ICoreInstanceOne one)
        {
            _actionResponse = true;
        }

        public bool DoAction() => _actionResponse;
    }
}
