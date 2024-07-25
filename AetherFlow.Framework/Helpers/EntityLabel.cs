using System;
using System.Linq;
using System.Reflection;
using AetherFlow.Framework.Attributes;

namespace AetherFlow.Framework.Helpers
{
    public static class EntityLabel
    {
        public static string ForField(Type fieldType, string fieldValue, int languageCode)
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

        public static string ForEnum(Type enumType, string enumValue, int languageCode)
        {
            var enumField = enumType.GetField(enumValue);
            var labelAttribute = enumField.GetCustomAttributes<LabelAttribute>()
                .FirstOrDefault(attr => attr.LanguageCode == languageCode);
            
            return labelAttribute?.Value ?? enumValue;
        }
    }
}
