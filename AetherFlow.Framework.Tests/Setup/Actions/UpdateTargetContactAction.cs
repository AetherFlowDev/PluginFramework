using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework.Tests.Setup.Actions.Config;
using AetherFlow.Framework.Tests.Setup.Interfaces.DataAccess;
using AetherFlow.Framework.Tests.Setup.Models;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework.Tests.Setup.Actions
{
    public class UpdateTargetContactAction : IPluginAction
    {
        private readonly IContactDal _contactDal;
        private readonly IPluginExecutionContext _context;
        private readonly ContactPluginConfig _config;

        public UpdateTargetContactAction(IContactDal contactDal, IPluginExecutionContext context, ContactPluginConfig config)
        {
            _contactDal = contactDal;
            _context = context;
            _config = config;
        }

        public void Execute()
        {
            var target = (Entity)_context.InputParameters["Target"];

            // Get the contact object, update values & save
            // Save actually updates the target object in this instance
            var contact = _contactDal.FromTarget(target);
            contact.StateCode = Contact.Choices.StateCode.Inactive;
            contact.FirstName = _config.FirstName;
            contact.Save();
        }
    }
}
