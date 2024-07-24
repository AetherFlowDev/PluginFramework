using System;
using System.Reflection;
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
        
        private JsonContractSerializer _serializer;
        private Assembly _assembly;

        public ActionExecutor(IDataverseContainer container, string secure, string unSecure)
        {
            Container = container;
            SecureConfig = secure;
            UnSecureConfig = unSecure;
        }

        public ActionExecutor FromAssembly(Assembly assembly)
        {
            _assembly = assembly;
            return this;
        }

        public ActionExecutor LoadDependencies(string rootNamespace)
        {
            Container.Initialize(_assembly ?? GetType().Assembly, rootNamespace);
            return this;
        }
        
        public ActionExecutor LoadDependencies(string[] rootNamespaces)
        {
            Container.Initialize(_assembly ?? GetType().Assembly, rootNamespaces);
            return this;
        }

        public ActionExecutor UseSecureConfig<T>() where T : new()
        {
            Container.Add<T>(GetSerializer().Deserialize<T>(SecureConfig));
            return this;
        }

        public ActionExecutor UseUnSecureConfig<T>() where T : new()
        {
            Container.Add<T>(GetSerializer().Deserialize<T>(UnSecureConfig));
            return this;
        }

        public ActionExecutor Run<T>() where T : IPluginAction
        {           
            var action = Container.Get<T>();
            action.Execute();
            return this;
        }

        protected IJsonSerializer GetSerializer()
        {
            return _serializer ?? (_serializer = new JsonContractSerializer());
        }

        public ActionExecutor RunIf<T>(Func<IPluginExecutionContext, bool> shouldRun) where T : IPluginAction
        {
            if (shouldRun(Container.Get<IPluginExecutionContext>())) Run<T>();
            return this;
        }
    }
}
