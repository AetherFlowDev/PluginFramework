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
                .LoadDependenciesFrom("AetherFlow.Framework.Tests.Content.Interfaces")
                .HasSecureConfig<ContactPluginConfig>()
                .OnlyIf(context => context.MessageName == "Create")
                .RunAction<UpdateTargetContactAction>();
    }
}
