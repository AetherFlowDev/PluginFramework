using System;

namespace AetherFlow.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class MainAttribute : Attribute
    {
        public int DefaultValue { get; }

        public MainAttribute(int defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}
