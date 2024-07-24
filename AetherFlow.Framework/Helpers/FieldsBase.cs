using System;
using System.Linq;
using AetherFlow.Framework.Attributes;

namespace AetherFlow.Framework.Helpers
{
    public static class FieldHelper
    {
        public static string GetFieldLabel(Type fieldSet, string fieldName, int languageCode)
        {
            var field = fieldSet.GetField(fieldName);
            if (!(field.GetCustomAttributes(typeof(Attributes.LabelAttribute), false) is LabelAttribute[] labelAttributes) || labelAttributes.Length == 0)
            {
                return fieldName;
            }

            var labelAttribute = labelAttributes.FirstOrDefault(a => a.LanguageCode == languageCode);
            return labelAttribute == null ? fieldName : labelAttribute.Value;
        }
    }
}
