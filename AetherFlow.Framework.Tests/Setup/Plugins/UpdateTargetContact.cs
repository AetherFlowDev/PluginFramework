using AetherFlow.Framework.Tests.Setup.Actions;
using AetherFlow.Framework.Tests.Setup.Actions.Config;

namespace AetherFlow.Framework.Tests.Setup.Plugins
{
    public class UpdateTargetContact : PluginBase
    {
        public UpdateTargetContact(string unSecure, string secure) : base(unSecure, secure) { }
        public UpdateTargetContact() { }

        protected override void Configure(ActionExecutor builder) =>
            builder
                .FromAssembly(GetType().Assembly)
                .LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces")
                .UseSecureConfig<ContactPluginConfig>()
                .RunIf<UpdateTargetContactAction>(context => context.MessageName == "Create");
    }
}
