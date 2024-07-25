using System;
using System.Collections.Generic;
using AetherFlow.FakeXrmEasy.Plugins;
using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using AetherFlow.Xml.Framework.Core.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests
{
    public class QueryPagerTests : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseAssembly(GetType().Assembly);
            this.LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces");
            this.UseFakeXrmEasy();
            this.RunSeeders();
            RunSpecification();
        }

        // ARRANGE variables
        IOrganizationService _service;
        IQueryPager _pager;

        public override void Arrange()
        {
            var container = this.GetContainer();
            _service = container.Get<IOrganizationService>();
            _pager = container.Get<IQueryPager>();
        }

        [Test]
        public void EnsureCanGetAllRecords()
        {
            var query = new QueryExpression("large") {
                ColumnSet = new ColumnSet(true)
            };
            
            var records = _service.RetrieveMultiple(query).Entities;
            Assert.That(records.Count, Is.EqualTo(100));
        }

        [Test]
        public void EnsureCanGetLimitedRecords()
        {
            var query = new QueryExpression("large") {
                ColumnSet = new ColumnSet(true),
                TopCount = 10
            };

            var records = _service.RetrieveMultiple(query).Entities;
            Assert.That(records.Count, Is.EqualTo(10));
        }

        [Test]
        public void EnsureCanGetPages()
        {
            var query = new QueryExpression("large") {
                ColumnSet = new ColumnSet(true)
            };

            var records = _pager.Page(query, 10).Entities;
            Assert.That(records.Count, Is.EqualTo(100));
        }

        [Test]
        public void EnsureCanPageIn()
        {
            var records = _pager.PageInQuery(
                "large",
                "value",
                new object[] { 1, 60, 80 },
                new ColumnSet(true),
                2
            );

            Assert.That(records.Entities.Count, Is.EqualTo(3));
        }

        [Test]
        public void EnsureEmptyValuesResultInNoRecords()
        {
            var records = _pager.PageInQuery(
                "large",
                "value",
                new object[] { },
                new ColumnSet(true),
                2
            );

            Assert.That(records.Entities.Count, Is.EqualTo(0));
        }
    }
}
