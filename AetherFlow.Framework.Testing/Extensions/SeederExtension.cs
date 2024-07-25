using AetherFlow.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AetherFlow.FakeXrmEasy.Plugins;
using AetherFlow.Framework.Testing.Attributes.Seeders;
using AetherFlow.Framework.Testing.Interfaces;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework.Testing.Extensions
{
    public static class SeederExtension
    {
        public static void UsesSeeders(this SpecificationBase spec)
        {
            // Get the services required
            var context = spec.GetXrmFakedContext();
            var assembly = spec.GetAssembly();

            // Get the seeders
            var seeders = assembly.GetTypes()
                .Where(t => typeof(IDataverseSeeder).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .OrderBy(GetSeedOrder)
                .Select(t => (IDataverseSeeder)Activator.CreateInstance(t))
                .ToList();

            // Run each seeder to extract the entities
            var entities = new List<Entity>();
            foreach (var seeder in seeders)
            {
                var newEntities = seeder.Execute(entities);
                entities.AddRange(newEntities);
            }

            // Add the entities to the context
            context.Initialize(entities);
        }

        private static int GetSeedOrder(Type type)
        {
            var order = type.GetCustomAttributes(typeof(SeedOrderAttribute), false)
                .FirstOrDefault() as SeedOrderAttribute;

            return order?.Order ?? 999999;
        }
    }
}
