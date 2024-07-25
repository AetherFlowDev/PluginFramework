using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework
{
    public abstract class EntityTracker
    {
        protected Guid? EntityGuid;
        protected readonly string LogicalName;
        protected IDictionary<string, object> Changes = new Dictionary<string, object>();
        protected readonly IDictionary<string, object> Values = new Dictionary<string, object>();

        protected EntityTracker(string logicalName, Guid? entityId = null, AttributeCollection attributes = null)
        {
            LogicalName = logicalName;
            EntityGuid = entityId;

            if (attributes == null) return;
            foreach (var attribute in attributes)
                Values.Add(attribute);
        }

        public Guid? Id
        {
            get => EntityGuid;
            set => EntityGuid = value;
        }

        public EntityReference GetReference()
        {
            if (Id == null)
                throw new Exception("Unable to generate an EntityReference of a transient entity");
            return new EntityReference(LogicalName, Id.Value);
        }

        public object this[string attributeName]
        {
            get =>
                Values.ContainsKey(attributeName)
                    ? Values[attributeName]
                    : null;
            set
            {
                // Set the current value & changes dictionaries 
                Values[attributeName] = value;
                Changes[attributeName] = value;
            }
        }

        public bool IsDirty() => Changes.Count > 0;

        protected Entity GetUpdatedEntity(bool clearChanges = false)
        {
            if (Changes.Count == 0) return null;

            var obj = new Entity(LogicalName);
            if (EntityGuid != null) obj.Id = EntityGuid.Value;
            obj.Attributes.AddRange(Changes);

            // Clear changes?
            if (clearChanges) Changes = new Dictionary<string, object>();

            return obj;
        }

        protected T? GetOptionSetValue<T>(string attributeName) where T : struct, Enum
        {
            if (!Values.ContainsKey(attributeName))
                return null;

            var value = this[attributeName];
            if (value is OptionSetValue osv)
                return (T)(object)osv.Value;

            return null;
        }

        protected void SetOptionSetValue<T>(string attributeName, T? value) where T : struct, Enum
        {
            var optionSetValue = value == null ? null : new OptionSetValue(Convert.ToInt32(value));
            this[attributeName] = optionSetValue;
        }

        protected void SetLookup(string attributeName, EntityReference value, string[] allowedEntities)
        {
            if (value == null)
            {
                this[attributeName] = null;
                return;
            }

            if (!allowedEntities.Contains(value.LogicalName))
                throw new Exception($"Entity {value.LogicalName} is not valid for attribute {attributeName}");

            this[attributeName] = value;
        }
    }
}
