using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace AetherFlow.Framework.Interfaces
{
    public interface IBulkExecutor
    {
        void AddRequest(OrganizationRequest request);
        void AddRequests(OrganizationRequest[] requests);
        void SetBatchSize(int batchSize);
        int Count();
        ReadOnlyDictionary<OrganizationRequest, string> Execute();
    }
}