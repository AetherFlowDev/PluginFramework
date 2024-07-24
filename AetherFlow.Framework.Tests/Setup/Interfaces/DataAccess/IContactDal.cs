using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using AetherFlow.Framework.Tests.Setup.Models;
using System.Web.Services.Description;

namespace AetherFlow.Framework.Tests.Setup.Interfaces.DataAccess
{
    public interface IContactDal
    {
        List<Contact> GetAllContacts();
        Contact GetContact(Guid contactId);
        Contact FromTarget(Entity e);
        Contact New();
        Contact New(Guid id);
    }
}