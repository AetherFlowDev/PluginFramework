using System;
using System.ComponentModel;
using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework.Processors;
using AetherFlow.Xml.Framework.Core.Interfaces;

namespace AetherFlow.Framework.Tests.Support.SpecificationExtensions
{
    public static class ContainerExtension
    {
        public static string DataName = "CONTAINER";

        public static void UseContainer(this SpecificationBase spec)
        {
            var container = new DataverseContainer();
            container.Add<IBulkExecutor, BulkExecutor>();
            container.Add<IQueryPager, QueryPager>();
            container.Add<ILog, Log>();

            spec.SetData(DataName, container);
        }

        public static void LoadDependencies(this SpecificationBase spec, string rootNamespace)
            => spec.LoadDependencies(new string[] { rootNamespace });

        public static void LoadDependencies(this SpecificationBase spec, string[] rootNamespaces)
        {
            var container = spec.GetContainer();
            var assembly = spec.GetAssembly();
            if (container == null || assembly == null)
                throw new Exception("No Container or Assembly set");

            container.Initialize(assembly, rootNamespaces);
        }

        public static IDataverseContainer GetContainer(this SpecificationBase spec)
            => (IDataverseContainer)spec.GetData(DataName) ?? throw new Exception("Container has not been initialized");

    }
}
