using AetherFlow.Framework.Interfaces;
using System.Reflection;

namespace AetherFlow.Framework.Tests.Support.SpecificationExtensions
{
    public static class AssemblyExtension
    {
        public static string DataName = "ASSEMBLY";

        public static void UseAssembly(this SpecificationBase spec, Assembly assembly)
            => spec.SetData(DataName, assembly);

        public static Assembly GetAssembly(this SpecificationBase spec)
            => (Assembly)spec.GetData(DataName) ?? throw new System.Exception("Assembly has not been initialized");
    }
}
