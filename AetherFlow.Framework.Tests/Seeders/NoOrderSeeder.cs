using System;
using System.Collections.Generic;
using AetherFlow.Framework.Testing.Interfaces;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework.Tests.Seeders
{
    public class NoOrderSeeder : IDataverseSeeder
    {
        public IList<Entity> Execute(IList<Entity> allRecords)
        {
            var records = new List<Entity>();
            records.Add(CreateRecord("1", allRecords.Count));
            return records;
        }

        private Entity CreateRecord(string name, int value)
        {
            return new Entity("noorder") {
                Id = Guid.NewGuid(),
                Attributes = {
                    { "name", name },
                    { "value", value }
                }
            };
        }
    }
}
