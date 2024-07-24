using System;
using System.Collections.Generic;
using AetherFlow.Framework.Tests.Support.Interfaces;

namespace AetherFlow.Framework.Tests
{
    public abstract class SpecificationBase : ISpecification, IDisposable
    {
        protected Exception ThrownException;
        protected Dictionary<string, object> data = new Dictionary<string, object>();

        public object GetData(string name) { return data.ContainsKey(name) ? data[name] : null; }
        public void SetData(string name, object value) {  data[name] = value; }
        
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
