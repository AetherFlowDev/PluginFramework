using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using AetherFlow.Framework.Tests.Setup.Interfaces.DataAccess;
using AetherFlow.Framework.Tests.Setup.Models;
using NUnit.Framework;
using System;
using System.Linq;
using AetherFlow.Framework.Helpers;
using Microsoft.Xrm.Sdk;

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
        public void EnsureCannotGetReferenceFromNewEntity()
        {
            var contact = _contactDal.New();
            Assert.Throws<Exception>(() => contact.GetReference());
        }

        [Test]
        public void EnsureCanSetIdManually()
        {
            var contact = _contactDal.New();
            contact.Id = _contact.Id;
            contact.FirstName = "EnsureCanSetIdManually";

            Assert.DoesNotThrow(() => contact.Save());
        }

        [Test]
        public void EnsureCanGetOptionSetValue()
        {
            _contact.StateCode = Contact.Choices.StateCode.Active;
            _contact.Save();
            Assert.That(_contact.StateCode, Is.EqualTo(Contact.Choices.StateCode.Active));
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

        [Test]
        public void EnsureCanSetCorrectLookup()
        {
            Assert.DoesNotThrow(() =>
                _contact.Account = new EntityReference("account", Guid.NewGuid())
            );
        }

        [Test]
        public void EnsureCannotSetIncorrectLookup()
        {
            Assert.Throws<Exception>(() =>
                _contact.Account = new EntityReference("settings", Guid.NewGuid())
            );
        }

        [Test]
        [TestCase(Contact.Fields.FirstName, 1033, "First Name")]
        [TestCase(Contact.Fields.FirstName, 1088, "Le First Name")]
        [TestCase(Contact.Fields.FirstName, 9999, "FirstName")]
        public void EnsureCanGetLabelFromField(string name, int language, string expected)
        {
            Assert.That(
                Contact.Fields.GetLabel(Contact.Fields.FirstName),
                Is.EqualTo("First Name")
            );
        }

        [Test]
        [TestCase(Contact.Choices.StateCode.LongerNameCode, 1033, "Longer Name Code")]
        [TestCase(Contact.Choices.StateCode.Active, 1088, "Le Active")]
        [TestCase(Contact.Choices.StateCode.LongerNameCode, 9999, "LongerNameCode")]
        public void EnsureCanGetLabelFromOptionSet(Contact.Choices.StateCode stateCode, int languageCode, string expected)
        {
            Assert.That(
                Contact.Choices.GetStateCodeLabel(stateCode, languageCode),
                Is.EqualTo(expected)
            );
        }

        [Test]
        public void EnsureCanExportEntity()
        {
            _contact.StateCode = Contact.Choices.StateCode.Inactive;
            var entity = _contact.Export();
            _contact.Save();
            
            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.LogicalName, Is.EqualTo(Contact.LogicalName));
            Assert.That(entity.Id, Is.EqualTo(_contact.Id ?? Guid.Empty));
            Assert.That(entity.Attributes.ContainsKey(Contact.Fields.FirstName), Is.True);
            Assert.That(entity.Attributes[Contact.Fields.FirstName], Is.EqualTo(_contact.FirstName));
            Assert.That(entity.Attributes.ContainsKey(Contact.Fields.StateCode), Is.True);
            Assert.That(((OptionSetValue)entity.Attributes[Contact.Fields.StateCode]).Value, Is.EqualTo((int)_contact.StateCode));
        }

        [Test]
        public void EnsureCanGetDefaultEnumValue()
        {
            Assert.That(
                EntityHelper.GetDefaultValue<Contact.Choices.StateCode>(), 
                Is.EqualTo(Contact.Choices.StateCode.Active)
            );
        }

        [Test]
        public void EnsureGettingEnumWithNoDefaultIsNull()
        {
            Assert.That(
                EntityHelper.GetDefaultValue<Contact.Choices.StatusCode>(),
                Is.Null
            );
        }
    }
}