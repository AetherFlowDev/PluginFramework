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

        public CoreInstanceThree(bool actionResponse)
        {
            _actionResponse = false;
        }

        public CoreInstanceThree(CoreInstanceOne one)
        {
            _actionResponse = false;
        }

        // Correct Constructor
        public CoreInstanceThree(ICoreInstanceOne one)
        {
            _actionResponse = true;
        }

        // Another Invalid one
        public CoreInstanceThree(CoreInstanceOne one, ICoreInstanceTwo two)
        {
            _actionResponse = false;
        }

        public bool DoAction() => _actionResponse;
    }
}
