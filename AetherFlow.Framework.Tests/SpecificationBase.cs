using System;
using AetherFlow.Framework.Tests.Interfaces;

namespace AetherFlow.Framework.Tests
{
    public abstract class SpecificationBase : ISpecification, IDisposable
    {
        protected Exception ThrownException;
        
        public virtual void RunSpecification()
        {
            Arrange();

            try { Act(); }
            catch (Exception ex) { ThrownException = ex; }
        }

        public virtual void Arrange()
        {
        }

        public virtual void Act()
        {
        }

        public virtual void Cleanup()
        {
        }

        public void Dispose()
        {
            Cleanup();
        }
    }
}
