using AetherFlow.Framework.Interfaces;
using AetherFlow.Framework.Processors;
using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using NUnit.Framework;
using System.Linq;

namespace AetherFlow.Framework.Tests.UnitTests.DataverseContainerTests
{
    public class CheckUniqueAttribute : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseFakeXrmEasy();
            this.UseAssembly(GetType().Assembly);
            this.LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces");
            RunSpecification();
        }

        private IDataverseContainer container;
        private IBulkExecutor bulkExecutorOne;
        private IBulkExecutor bulkExecutorTwo;
        private BulkExecutor conBulkOne;
        private BulkExecutor conBulkTwo;

        public override void Arrange()
        {
            container = this.GetContainer();
            bulkExecutorOne = container.Get<IBulkExecutor>();
            conBulkOne = container.Get<BulkExecutor>();
        }

        public override void Act()
        {
            bulkExecutorOne.AddRequest(new Microsoft.Xrm.Sdk.OrganizationRequest());
            conBulkOne.AddRequest(new Microsoft.Xrm.Sdk.OrganizationRequest());
            bulkExecutorTwo = container.Get<IBulkExecutor>();
            conBulkTwo = container.Get<BulkExecutor>();
        }

        [Test]
        public void AssertOneRequestForFirst()
        {
            Assert.That(bulkExecutorOne.Count, Is.EqualTo(1));
            Assert.That(conBulkOne.Count, Is.EqualTo(1));
        }

        [Test]
        public void AssertZeroRequestForSecond()
        {
            Assert.That(bulkExecutorTwo.Count, Is.EqualTo(0));
            Assert.That(conBulkTwo.Count, Is.EqualTo(0));
        }
    }
}
