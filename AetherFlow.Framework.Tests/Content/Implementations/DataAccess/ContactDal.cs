using System;
using System.Collections.Generic;
using System.Linq;
using AetherFlow.Xml.Framework.Core.Interfaces;
using AetherFlow.Framework.Tests.Content.Interfaces;
using AetherFlow.Framework.Tests.Content.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace AetherFlow.Framework.Tests.Content.Implementations.DataAccess
{
    public class ContactDal : IContactDal
    {
        private readonly IOrganizationService _service;
        private readonly IQueryPager _pager;

        public ContactDal(IOrganizationService service, IQueryPager pager)
        {
            _service = service;
            _pager = pager;
        }

        public List<Contact> GetAllContacts()
        {
            var query = new QueryExpression(Contact.LogicalName) {
                ColumnSet = new ColumnSet(true)
            };

            var entities = _pager.Page(query, 1000);
            return entities.Entities.Select(a => new Contact(a, _service)).ToList();
        }

        public Contact GetContact(Guid contactId)
        {
            var entity = _service.Retrieve(Contact.LogicalName, contactId, new ColumnSet(true));
            return new Contact(entity, _service);
        }

        public Contact FromTarget(Entity e)
        {
            var contact = new Contact(e, _service);
            contact.RegisterAsPreOperation(e);
            return contact;
        }
    }
}
