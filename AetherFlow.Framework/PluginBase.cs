using System;
using AetherFlow.Xml.Framework.Core.Interfaces;
using AetherFlow.Framework.Configuration;
using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework.Processors;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework
{
    public abstract class PluginBase : IPlugin
    {
        protected readonly string SecureConfig = "";
        protected readonly string UnSecureConfig = "";

        protected PluginBase() { }
        protected PluginBase(string unSecure, string secure)
        {
            UnSecureConfig = unSecure;
            SecureConfig = secure;
        }

        protected virtual void Configure(ActionExecutor builder) { }

        public void Execute(IServiceProvider serviceProvider)
        {
            // Get the associated services
            var executionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var notificationService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var organizationService = serviceFactory.CreateOrganizationService(executionContext.UserId);

            // Check to see if we need a new container or if we have been
            // passed one, for instance, from unit testing
            IDataverseContainer container = new DataverseContainer();
            if (executionContext.InputParameters.ContainsKey("DataverseContainer"))
                container = (IDataverseContainer)executionContext.InputParameters["DataverseContainer"];

            // Add services to the container
            container.Add<IServiceProvider>(serviceProvider);
            container.Add<IPluginExecutionContext>(executionContext);
            container.Add<ITracingService>(tracingService);
            container.Add<IServiceEndpointNotificationService>(notificationService);
            container.Add<IOrganizationServiceFactory>(serviceFactory);
            container.Add<IOrganizationService>(organizationService);
            //container.Add<ITraceConfiguration>(new TraceConfiguration());

            // Add generic interfaces to the container
            // Then get an instance of the log object
            RegisterGenericProcessors(container);
            var log = container.Get<ILog>();

            // Now, lets run the execute command, but wrap in a try catch
            try
            {
                Configure(new ActionExecutor(container, SecureConfig, UnSecureConfig));
            }
            catch (Exception ex)
            {               
                // Ignore correctly thrown exception
                if (ex is InvalidPluginExecutionException) throw;

                // Deal with fatal unexpected exception
                log.Fatal("Unexpected Error: " + ex.Message, ex);
                throw new InvalidPluginExecutionException(
                    OperationStatus.Failed, 
                    "An unexpected error occurred - " + ex.Message
                );
            }
        }

        private void RegisterGenericProcessors(IDataverseContainer container)
        {
            container.Add<IBulkExecutor, BulkExecutor>();
            container.Add<IQueryPager, QueryPager>();
            container.Add<ILog, Log>();
        }
    }
}
