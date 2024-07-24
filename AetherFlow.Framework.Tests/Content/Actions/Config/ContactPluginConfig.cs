﻿using System.Runtime.Serialization;
using AetherFlow.Framework.Interfaces;

namespace AetherFlow.Framework.Tests.Content.Actions.Config
{
    [DataContract]
    public class ContactPluginConfig : IConfiguration
    {
        [DataMember]
        public string FirstName { get; set; }
    }
}
