using AetherFlow.Framework.Tests.Setup.Interfaces.DataAccess;
using AetherFlow.Framework.Tests.Setup.Models;
using AetherFlow.Framework.Tests.Support.SpecificationExtensions;
using NUnit.Framework;
using System;
using System.Linq;

namespace AetherFlow.Framework.Tests.UnitTests
{
    public class EntityTests : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseAssembly(GetType().Assembly);
            this.LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces");
            this.UseFakeXrmEasy();
            RunSpecification();
        }

        // ARRANGE variables
        IContactDal _contactDal;

        // ACT variables
        Contact _contact;

        public override void Arrange()
        {
            var container = this.GetContainer();
            _contactDal = container.Get<IContactDal>();
        }

        public override void Act()
        {
            _contact = _contactDal.New();
            _contact.FirstName = "FirstName";
            _contact.Save();
        }

        [Test]
        public void EnsureContactHasAnId()
        {
            var contact = _contactDal.New();
            contact.FirstName = "Test";
            contact.Save();

            Assert.That(contact.Id, Is.Not.Null);
        }

        [Test]
        public void EnsureCanGetValueFromRecordManually()
        {
            _contact.FirstName = "Test";
            _contact.Save();

            Assert.That(_contact[Contact.Fields.FirstName], Is.EqualTo("Test"));
        }

        [Test]
        public void EnsureContactDetailsAreCorrect()
        {
            var contact = _contactDal.New();
            contact.FirstName = "Test";
            contact.Save();

            var context = this.GetXrmFakedContext();
            var contactFromDb = context.Data[Contact.LogicalName].First(a => a.Key == contact.Id).Value;
            Assert.That(contactFromDb.GetAttributeValue<string>(Contact.Fields.FirstName), Is.EqualTo("Test"));
        }

        [Test]
        public void EnsureCanUpdate()
        {
            _contact.FirstName = "Updated";
            _contact.Save();

            var context = this.GetXrmFakedContext();
            var contactFromDb = context.Data[Contact.LogicalName].First(a => a.Key == _contact.Id).Value;
            Assert.That(contactFromDb.GetAttributeValue<string>(Contact.Fields.FirstName), Is.EqualTo("Updated"));
        }

        [Test]
        public void EnsureCanDelete()
        {
            var contact = _contactDal.New();
            contact.FirstName = "Test";
            contact.Save();
            contact.Delete();

            var context = this.GetXrmFakedContext();
            var contactFromDb = context.Data[Contact.LogicalName].FirstOrDefault(a => a.Key == contact.Id).Value;
            Assert.That(contactFromDb, Is.Null);
        }

        [Test]
        public void EnsureIsDirtyCorrectOnUpdateAndSave()
        {
            Assert.That(_contact.IsDirty(), Is.False);
            _contact.FirstName = "Richard";
            Assert.That(_contact.IsDirty(), Is.True);
            _contact.Save();
            Assert.That(_contact.IsDirty(), Is.False);
        }

        [Test]
        public void EnsureEntityReferenceReturned()
        {
            var entityReference = _contact.GetReference();

            Assert.That(entityReference, Is.Not.Null);
            Assert.That(entityReference.Id, Is.EqualTo(_contact.Id));
            Assert.That(entityReference.LogicalName, Is.EqualTo(Contact.LogicalName));
        }

        [Test]
        public void EnsureCreateRequestReturned()
        {
            var contact = _contactDal.New();
            contact.FirstName = "Test";
            var request = contact.CreateRequest();

            Assert.That(request, Is.Not.Null);
            Assert.That(request.Target.LogicalName, Is.EqualTo(Contact.LogicalName));
        }

        [Test]
        public void EnsureCantCreateRequestCreatedRecord()
        {
            var request = _contact.CreateRequest();
            Assert.That(request, Is.Null);
        }

        [Test]
        public void EnsureUpdateRequestReturned()
        {
            _contact.FirstName = "UpdateRequest";
            var request = _contact.UpdateRequest();
            _contact.Save();

            Assert.That(request, Is.Not.Null);
            Assert.That(request.Target.Id, Is.EqualTo(_contact.Id));
            Assert.That(request.Target.LogicalName, Is.EqualTo(Contact.LogicalName));
        }

        [Test]
        public void EnsureCantUpdateRequestNotCreatedRecord()
        {
            var contact = _contactDal.New();
            contact.FirstName = "Test";
            var request = contact.UpdateRequest();

            Assert.That(request, Is.Null);
        }

        [Test]
        public void EnsureCantUpdateRequestNonDirtyRecord()
        {
            var contact = _contactDal.New();
            contact.FirstName = "Richard";
            contact.Save();

            var request = contact.UpdateRequest();
            Assert.That(request, Is.Null);
        }

        [Test]
        public void EnsureDeleteRequestReturned()
        {
            var request = _contact.DeleteRequest();

            Assert.That(request, Is.Not.Null);
            Assert.That(request.Target.Id, Is.EqualTo(_contact.Id));
            Assert.That(request.Target.LogicalName, Is.EqualTo(Contact.LogicalName));
        }

        [Test]
        public void EnsureCantDeleteRequestNonCreatedRecord()
        {
            var contact = _contactDal.New();
            contact.FirstName = "Test";
            var request = contact.DeleteRequest();

            Assert.That(request, Is.Null);
        }

        [Test]
        public void EnsureCanGetAttributes()
        {
            var contact = _contactDal.New(_contact.Id.Value);
            contact.Get(new string[] { Contact.Fields.FirstName });

            Assert.That(contact.FirstName, Is.EqualTo(_contact.FirstName));
        }

        [Test]
        public void EnsureExceptionThrownWhenSavingAndNoServiceObject()
        {
            var contact = new Contact();
            contact.FirstName = "Test";
            Assert.Throws<Exception>(() => contact.Save());
        }

        [Test]
        public void EnsureGettingDataFromNoIdDoesNothing()
        {
            var contact = _contactDal.New();
            contact.Get(new string[] { Contact.Fields.FirstName });
            Assert.That(contact.FirstName, Is.Null);
        }

        [Test]
        public void EnsureDataIsUpdatedWithGetRequest()
        {
            var contact = _contactDal.New(_contact.Id.Value);
            contact.FirstName = "EnsureDataIsUpdateWithGetRequest";
            contact.Save();

            _contact.Get(new string[] { Contact.Fields.FirstName });

            Assert.That(_contact.FirstName, Is.EqualTo(contact.FirstName));
        }

        [Test]
        public void EnsureGetRemovesChanges()
        {
            var contact = _contactDal.New(_contact.Id.Value);
            contact.FirstName = "EnsureGetRemovesChanges";
            contact.Get(new string[] { Contact.Fields.FirstName });

            Assert.That(contact.IsDirty(), Is.False);
            Assert.That(contact.FirstName, Is.Not.EqualTo("EnsureGetRemovesChanges"));
        }
    }
}