using System;

namespace AetherFlow.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class VariantAttribute : Attribute
    {
        public string Name { get; }

        public VariantAttribute(string name)
        {
            Name = name;
        }
    }
}
