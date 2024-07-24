using System;
using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework.Tests.Content.Actions.Config;
using AetherFlow.Framework.Tests.Content.Implementations.DataAccess;
using AetherFlow.Framework.Tests.Content.Interfaces;
using AetherFlow.Framework.Tests.Content.Models;
using Microsoft.Xrm.Sdk;

namespace AetherFlow.Framework.Tests.Content.Actions
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
