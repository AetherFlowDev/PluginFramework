using System;

namespace AetherFlow.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class LabelAttribute : Attribute
    {
        public int LanguageCode { get; }
        public string Value { get; }

        public LabelAttribute(int languageCode, string value)
        {
            LanguageCode = languageCode;
            Value = value;
        }
    }
}
