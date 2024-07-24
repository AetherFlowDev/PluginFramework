using System;
using AetherFlow.Framework.Attributes;
using AetherFlow.Framework.Helpers;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework.Tests.Content.Models
{
    public class Contact : EntityBase
    {
        public static new readonly string LogicalName = "contact";

        public static readonly string SchemaName = "Contact";
        public static readonly string IdAttribute = "contactid";
        public static readonly string PrimaryAttribute = "fullname";
        public static readonly string CollectionName = "contacts";

        public Contact(Entity record, IOrganizationService service) : base(LogicalName, record, service) { }
        public Contact(IOrganizationService service = null) : base(LogicalName, service) { }
        public Contact(Guid id, IOrganizationService service) : base(LogicalName, id, service) { }

        public static class Fields
        {
            [Label(1033, "Full Name")]
            [Label(1088, "Le Full Name")]
            public const string FirstName = "firstname";
            
            [Label(1033, "State")]
            [Label(1088, "Le State")]
            public const string StateCode = "statecode";

            [Label(1033, "Account")]
            [Label(1088, "Le Account")]
            public const string Account = "accountid";

            public static string GetLabel(string fieldName, int languageCode = 1033)
                => FieldHelper.GetFieldLabel(typeof(Fields), fieldName, languageCode);
        }

        public static class Choices
        {
            [Default(0)]
            public enum StateCode
            {
                [Label(1033, "Active")]
                [Label(1088, "Le Active")]
                Active = 0,

                [Label(1033, "Inactive")]
                [Label(1088, "Le Inactive")]
                Inactive = 1
            }
        }

        public string FirstName
        {
            get => (string)this[Fields.FirstName];
            set => this[Fields.FirstName] = value;
        }
        
        public Choices.StateCode? StateCode
        {
            get => GetOptionSetValue<Choices.StateCode>(Fields.StateCode);
            set => SetOptionSetValue(Fields.StateCode, value);
        }
        
        public EntityReference Account
        {
            get => (EntityReference)this[Fields.Account];
            set => SetLookup(Fields.Account, value, new[] { "account" });
        }
    }
}
