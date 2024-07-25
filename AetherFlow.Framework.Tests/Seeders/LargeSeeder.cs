using System;
using System.Collections.Generic;
using AetherFlow.Framework.Testing.Attributes.Seeders;
using AetherFlow.Framework.Testing.Interfaces;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework.Tests.Seeders
{
    [SeedOrder(100)]
    public class LargeSeeder : IDataverseSeeder
    {
        public IList<Entity> Execute(IList<Entity> allRecords)
        {
            var records = new List<Entity>();
            for (int i = 0; i < 100; i++)
            {
                records.Add(CreateRecord("large", i));
            }
            return records;
        }

        private Entity CreateRecord(string name, int value)
        {
            return new Entity("large") {
                Id = Guid.NewGuid(),
                Attributes = {
                    { "name", name },
                    { "value", value }
                }
            };
        }
    }
}
