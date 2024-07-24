using AetherFlow.Framework.Tests.Content.Actions;
using AetherFlow.Framework.Tests.Content.Actions.Config;

namespace AetherFlow.Framework.Tests.Content.Plugins
{
    public class UpdateTargetContact : PluginBase
    {
        public UpdateTargetContact(string unSecure, string secure) : base(unSecure, secure) { }
        public UpdateTargetContact() { }

        protected override void Configure(ActionExecutor builder) =>
            builder
                .FromAssembly(GetType().Assembly)
                .LoadDependencies("AetherFlow.Framework.Tests.Content.Interfaces")
                .UseSecureConfig<ContactPluginConfig>()
                .RunIf<UpdateTargetContactAction>(context => context.MessageName == "Create");
    }
}
