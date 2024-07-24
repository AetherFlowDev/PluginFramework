using System;
using System.Collections.Generic;
using System.Linq;
using AetherFlow.Xml.Framework.Core.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace AetherFlow.Framework.Processors
{
    public class QueryPager : IQueryPager
    {
        private readonly IOrganizationService _service;

        public QueryPager(IOrganizationService service)
        {
            _service = service;
        }

        public EntityCollection Page(QueryExpression qe, int perPage)
        {
            // Setup paging info
            qe.PageInfo = new PagingInfo
            {
                Count = perPage,
                PageNumber = 1
            };

            // Somewhere to store the results
            var records = new EntityCollection();

            // Loop for more results
            while (true)
            {
                var results = _service.RetrieveMultiple(qe);
                records.Entities.AddRange(results.Entities);

                // Anymore records?
                if (!results.MoreRecords)
                    break;

                // Guess not, move to the next page!
                qe.PageInfo.PageNumber++;
                qe.PageInfo.PagingCookie = results.PagingCookie;
            }

            // Set metadata, return the records
            records.EntityName = qe.EntityName;
            records.TotalRecordCount = records.Entities.Count;
            return records;
        }

        public EntityCollection PageInQuery(string entityName, string fieldName, List<Guid> values, ColumnSet columnSet, int perPage)
        {
            // We need to setup an IN query.
            // This needs to use a dynamic condition, as defined in the parameters 
            // Lets setup some values to use!
            var page = 0;
            var ec = new EntityCollection {
                EntityName = entityName,
                TotalRecordCount = 0
            };

            // We might not have anything to do!
            if (values.Count == 0)
                return ec;

            // Start the loop!
            while (true)
            {
                // Setup the query
                var guids = values.Skip(page * perPage).Take(perPage);
                var enumerable = guids as Guid[] ?? guids.ToArray();
                if (!enumerable.Any())
                    break;

                var qe = new QueryExpression {
                    EntityName = entityName,
                    ColumnSet = columnSet,
                    Criteria = {
                        Conditions = {
                            new ConditionExpression(fieldName, ConditionOperator.In, enumerable.ToArray())
                        }
                    },
                    NoLock = true
                };

                // execute the query
                var results = _service.RetrieveMultiple(qe);

                // Add the results!
                ec.Entities.AddRange(results.Entities);

                // Increment the page
                page++;

                // Ensure we have not hit the end
                if (page * perPage > values.Count)
                    break;
            }

            // Final, update the entity collection and return it
            ec.TotalRecordCount = ec.Entities.Count;
            return ec;
        }
    }
}
