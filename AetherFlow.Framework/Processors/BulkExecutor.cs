using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AetherFlow.Framework.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace AetherFlow.Framework.Processors
{
    public class BulkExecutor : IBulkExecutor
    {
        private int _batchSize = 2000;
        private readonly Dictionary<OrganizationRequest, string> _errors = new Dictionary<OrganizationRequest, string>();
        private readonly List<OrganizationRequest> _requests = new List<OrganizationRequest>();
        private readonly IOrganizationService _service;

        public BulkExecutor(IOrganizationService service)
        {
            _service = service;
        }

        public ReadOnlyDictionary<OrganizationRequest, string> GetErrors()
            => new ReadOnlyDictionary<OrganizationRequest, string>(_errors);

        public void AddRequest(OrganizationRequest request) 
            => _requests.Add(request);
        
        public void AddRequests(IList<OrganizationRequest> requests) 
            => _requests.AddRange(requests);

        public void SetBatchSize(int batchSize)
            => _batchSize = batchSize;

        public int Count() 
            => _requests.Count;

        public void Execute()
        {
            var moreRecords = Count() > 0;
            var count = 0;

            while (moreRecords)
            {
                var request = new ExecuteMultipleRequest
                {
                    Settings = new ExecuteMultipleSettings
                    {
                        ContinueOnError = true,
                        ReturnResponses = true
                    },
                    Requests = new OrganizationRequestCollection()
                };

                request.Requests.AddRange(
                    _requests.Skip(count).Take(_batchSize)
                );

                count += _batchSize;
                if (count > _requests.Count)
                    moreRecords = false;

                // Execute the request;
                var response = (ExecuteMultipleResponse)_service.Execute(request);

                // Identify failed requests
                foreach (var item in response.Responses.Where(item => item.Fault != null))
                {
                    _errors.Add(
                        request.Requests[item.RequestIndex],
                        item.Fault.Message
                    );
                }
            }
        }
    }
}
