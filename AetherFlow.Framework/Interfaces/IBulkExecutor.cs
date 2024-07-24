using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework.Interfaces
{
    public interface IBulkExecutor
    {
        ReadOnlyDictionary<OrganizationRequest, string> GetErrors();
        void AddRequest(OrganizationRequest request);
        void AddRequests(IList<OrganizationRequest> requests);
        void SetBatchSize(int batchSize);
        int Count();
        void Execute();
    }
}