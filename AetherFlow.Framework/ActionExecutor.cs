using System;
using AetherFlow.Framework.Helpers;
using AetherFlow.Framework.Interfaces;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework
{
    public class ActionExecutor
    {
        protected static IDataverseContainer Container;
        protected static string SecureConfig;
        protected static string UnSecureConfig;
        protected bool RunActions = true;
        
        private JsonContractSerializer _serializer;

        public ActionExecutor(IDataverseContainer container, string secure, string unSecure)
        {
            Container = container;
            SecureConfig = secure;
            UnSecureConfig = unSecure;
        }

        public ActionExecutor LoadDependenciesFrom(string rootNamespace)
        {
            Container.Initialize(GetType().Assembly, rootNamespace);
            return this;
        }
        
        public ActionExecutor LoadDependenciesFrom(string[] rootNamespaces)
        {
            Container.Initialize(GetType().Assembly, rootNamespaces);
            return this;
        }

        public ActionExecutor HasSecureConfig<T>() where T : new()
        {
            Container.Add<T>(GetSerializer().Deserialize<T>(SecureConfig));
            return this;
        }

        public ActionExecutor HasUnSecureConfig<T>() where T : new()
        {
            Container.Add<T>(GetSerializer().Deserialize<T>(UnSecureConfig));
            return this;
        }

        public ActionExecutor RunAction<T>() where T : IPluginAction
        {
            if (!RunActions) return this;
            
            var action = Container.Get<T>();
            action.Execute();
            return this;
        }

        protected IJsonSerializer GetSerializer()
        {
            return _serializer ?? (_serializer = new JsonContractSerializer());
        }

        public ActionExecutor OnlyIf(Func<IPluginExecutionContext, bool> shouldRun)
        {
            RunActions = shouldRun(Container.Get<IPluginExecutionContext>());
            return this;
        }
    }
}
