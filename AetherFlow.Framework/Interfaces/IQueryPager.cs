using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace AetherFlow.Xml.Framework.Core.Interfaces
{
    public interface IQueryPager
    {
        EntityCollection Page(QueryExpression qe, int perPage);
        EntityCollection PageInQuery(string entityName, string fieldName, List<Guid> values, ColumnSet columnSet, int perPage);
    }
}