using System;

namespace AetherFlow.Framework.Testing.Attributes.Seeders
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SeedOrderAttribute : Attribute
    {
        public int Order { get; }

        public SeedOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
