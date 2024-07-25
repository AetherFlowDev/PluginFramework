using AetherFlow.FakeXrmEasy.Plugins;
using AetherFlow.Framework.Interfaces;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework.Testing.Extensions
{
    public static class XrmFakedExtension
    {
        public static string DataName = "FAKEXRMEASY";

        public static void UseFakeXrmEasy(this SpecificationBase spec)
        {
            var context = new XrmFakedContext();
            spec.SetData(DataName, context);

            // Get the container, and inject the services
            // only if container is set
            var obj = spec.GetData(ContainerExtension.DataName);
            if (obj == null) return;

            // We have a container, load up with plugin services
            var container = (IDataverseContainer)obj;
            container.Add<IOrganizationService>(context.GetOrganizationService());
            container.Add<ITracingService>(context.GetFakeTracingService());
            container.Add<IServiceEndpointNotificationService>(context.GetFakedServiceEndpointNotificationService());
        }

        public static XrmFakedContext GetXrmFakedContext(this SpecificationBase spec)
            => (XrmFakedContext)spec.GetData(DataName) ?? throw new System.Exception("FakeXrmEasy has not been initalized");
    }
}
