using System;
using System.Linq;
using System.Reflection;
using AetherFlow.Framework.Attributes;

namespace AetherFlow.Framework.Extensions
{
    public static class EnumExtensions
    {
        public static string ToLabel(this Enum value, int languageCode)
        {
            var field = value.GetType().GetField(value.ToString());
            if (!(field.GetCustomAttributes(typeof(Attributes.LabelAttribute), false) is LabelAttribute[] labelAttributes) || labelAttributes.Length == 0)
            {
                return value.ToString();
            }

            var labelAttribute = labelAttributes.FirstOrDefault(a => a.LanguageCode == languageCode);
            return labelAttribute == null ? value.ToString() : labelAttribute.Value;
        }

        public static int? GetDefaultValue<T>() where T : Enum
        {
            // Check if the enum type has the DefaultAttribute
            var defaultAttr = typeof(T).GetCustomAttribute<DefaultAttribute>();
            return defaultAttr?.DefaultValue;
        }
    }
}
