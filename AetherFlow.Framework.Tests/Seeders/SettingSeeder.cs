using System;
using System.Collections.Generic;
using AetherFlow.Framework.Testing.Attributes.Seeders;
using AetherFlow.Framework.Testing.Interfaces;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework.Tests.Seeders
{
    [SeedOrder(2)]
    public class SecondarySeeder : IDataverseSeeder
    {
        public IList<Entity> Execute(IList<Entity> allRecords)
        {
            var records = new List<Entity>();
            records.Add(CreateRecord("2", allRecords.Count));
            return records;
        }

        private Entity CreateRecord(string name, int value)
        {
            return new Entity("secondary") {
                Id = Guid.NewGuid(),
                Attributes = {
                    { "name", name },
                    { "value", value }
                }
            };
        }
    }
}
