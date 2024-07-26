using System.Runtime.Serialization;
using AetherFlow.Framework.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Actions.Config
{
    [DataContract]
    public class ContactPluginConfig
    {
        [DataMember]
        public string FirstName { get; set; }
    }
}
