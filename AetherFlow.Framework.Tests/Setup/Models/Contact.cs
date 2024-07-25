using System;
using AetherFlow.Framework.Attributes;
using AetherFlow.Framework.Helpers;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework.Tests.Setup.Models
{
    public class Contact : EntityBase
    {
        public new static readonly string LogicalName = "contact";

        public static readonly string SchemaName = "Contact";
        public static readonly string IdAttribute = "contactid";
        public static readonly string PrimaryAttribute = "fullname";
        public static readonly string CollectionName = "contacts";

        public Contact(Entity record, IOrganizationService service) : base(LogicalName, record, service) { }
        public Contact(IOrganizationService service = null) : base(LogicalName, service) { }
        public Contact(Guid id, IOrganizationService service) : base(LogicalName, id, service) { }
        
        public static class Fields
        {
            [Label(1033, "First Name")]
            [Label(1088, "Le First Name")]
            public const string FirstName = "firstname";

            [Label(1033, "State")]
            [Label(1088, "Le State")]
            public const string StateCode = "statecode";

            [Label(1033, "Account")]
            [Label(1088, "Le Account")]
            public const string Account = "accountid";

            public static string GetLabel(string fieldValue, int languageCode = 1033)
                => EntityLabel.ForField(typeof(Fields), fieldValue, languageCode);
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
                Inactive = 1,

                [Label(1033, "Longer Name Code")]
                [Label(1088, "Le Longer Name Code")]
                LongerNameCode = 2
            }

            public static string GetStateCodeLabel(StateCode stateCode, int languageCode = 1033)
                => EntityLabel.ForEnum(typeof(StateCode), stateCode.ToString(), languageCode);
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
