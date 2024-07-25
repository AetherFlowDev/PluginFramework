using System;
using System.Collections.Generic;
using AetherFlow.Framework.Testing.Attributes.Seeders;
using AetherFlow.Framework.Testing.Interfaces;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework.Tests.Seeders
{
    [SeedOrder(1)]
    public class SettingSeeder : IDataverseSeeder
    {
        public IList<Entity> Execute(IList<Entity> allRecords)
        {
            var records = new List<Entity>();
            records.Add(CreateSetting("1", allRecords.Count));
            return records;
        }

        private Entity CreateSetting(string name, int value)
        {
            return new Entity("setting") {
                Id = Guid.NewGuid(),
                Attributes = {
                    { "name", name },
                    { "value", value }
                }
            };
        }
    }
}
