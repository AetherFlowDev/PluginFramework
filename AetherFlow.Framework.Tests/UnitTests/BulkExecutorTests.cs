using System.Collections.Generic;
using System.Linq;
using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using AetherFlow.Framework.Tests.Setup.Models;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests
{
    public class BulkExecutorTests : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseFakeXrmEasy();
            RunSpecification();
        }

        // ARRANGE variables
        private IOrganizationService _service;
        private IBulkExecutor _bulkExecutor;

        // ACT variables
        private IReadOnlyDictionary<OrganizationRequest, string> _errors;

        public override void Arrange()
        {
            var container = this.GetContainer();
            _service = container.Get<IOrganizationService>();
            _bulkExecutor = container.Get<IBulkExecutor>();

            _bulkExecutor.SetBatchSize(10);
            _bulkExecutor.AddRequests(
                Enumerable.Range(0, 100)
                    .Select(i => new Contact(_service) { FirstName = $"Contact {i}" })
                    .Select(a => a.CreateRequest())
                    .ToArray<OrganizationRequest>()
            );

            // Add a failing request
            var contact = new Contact(_service) {
                FirstName = "Failing Contact", StateCode = Contact.Choices.StateCode.Active
            };

            _bulkExecutor.AddRequest(contact.CreateRequest());
        }

        public override void Act()
        {
            _errors = _bulkExecutor.Execute();
        }

        [Test]
        public void EnsureThrowsNoException()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void EnsureOneHundredContactsCreated()
        {
            var context = this.GetXrmFakedContext();
            Assert.That(context.Data[Contact.LogicalName].Count, Is.EqualTo(100));
        }

        [Test]
        public void EnsureSingleErrorReturned()
        {
            Assert.That(_errors.Count, Is.EqualTo(1));
        }
    }
}
