using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using AetherFlow.Framework.Tests.Content.Models;

namespace AetherFlow.Framework.Tests.Content.Interfaces
{
    public interface IContactDal
    {
        List<Contact> GetAllContacts();
        Contact GetContact(Guid contactId);
        Contact FromTarget(Entity e);
    }
}