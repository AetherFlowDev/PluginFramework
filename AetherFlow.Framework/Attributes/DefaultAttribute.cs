using System;

namespace AetherFlow.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class DefaultAttribute : Attribute
    {
        public int DefaultValue { get; }

        public DefaultAttribute(int defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}
