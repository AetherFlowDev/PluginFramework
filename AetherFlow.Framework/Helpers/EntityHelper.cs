using System;
using System.Linq;
using System.Reflection;
using AetherFlow.Framework.Attributes;

namespace AetherFlow.Framework.Helpers
{
    public static class EntityHelper
    {
        public static string GetFieldLabel(Type fieldType, string fieldValue, int languageCode)
        {
            var field = fieldType.GetFields(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(f => f.GetValue(null)?.ToString() == fieldValue);

            if (field == null)
            {
                throw new ArgumentException($"Field with value '{fieldValue}' not found in type '{fieldType.Name}'.");
            }

            var labelAttribute = field.GetCustomAttributes<LabelAttribute>()
                .FirstOrDefault(attr => attr.LanguageCode == languageCode);

            return labelAttribute?.Value ?? field.ToString();
        }

        public static string GetEnumLabel(Type enumType, string enumValue, int languageCode)
        {
            var enumField = enumType.GetField(enumValue);
            var labelAttribute = enumField.GetCustomAttributes<LabelAttribute>()
                .FirstOrDefault(attr => attr.LanguageCode == languageCode);
            
            return labelAttribute?.Value ?? enumValue;
        }

        public static T? GetDefaultValue<T>() where T : struct, Enum
        {
            var enumType = typeof(T);
            var defaultAttribute = (DefaultAttribute)Attribute.GetCustomAttribute(enumType, typeof(DefaultAttribute));

            // Where no default value, use null
            if (defaultAttribute == null) return null;
            
            // Where we HAVE a default value, get it and return the enum
            var defaultValue = defaultAttribute.DefaultValue;
            return (T)Enum.ToObject(enumType, defaultValue);
        }
    }
}
