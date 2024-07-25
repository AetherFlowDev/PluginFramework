using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework.Testing.Interfaces
{
    public interface IDataverseSeeder
    {
        IList<Entity> Execute(IList<Entity> allRecords);
    }
}
