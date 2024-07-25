using System.Linq;
using AetherFlow.Framework.Testing;
using NUnit.Framework;
using AetherFlow.Framework.Testing.Extensions;

namespace AetherFlow.Framework.Tests.UnitTests
{
    internal class SeederTests : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseAssembly(GetType().Assembly);
            this.LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces");
            this.UseFakeXrmEasy();
            this.UsesSeeders();
            RunSpecification();
        }

        [Test]
        public void EnsureRecordsAreCreated()
        {
            var context = this.GetXrmFakedContext();
            Assert.That(context.Data.ContainsKey("setting"), Is.True);
            Assert.That(context.Data.ContainsKey("secondary"), Is.True);
        }

        [Test]
        public void EnsureRecordCountsAreCorrect()
        {
            var context = this.GetXrmFakedContext();
            Assert.That(context.Data?["setting"]?.Count ?? 0, Is.EqualTo(1));
            Assert.That(context.Data?["secondary"]?.Count ?? 0, Is.EqualTo(1));
        }

        [Test]
        public void EnsureRecordsCreatedInCorrectOrder()
        {
            var context = this.GetXrmFakedContext();
            Assert.That((int)context.Data["setting"].First().Value.Attributes["value"], Is.EqualTo(0));
            Assert.That((int)context.Data["secondary"].First().Value.Attributes["value"], Is.EqualTo(1));
            Assert.That((int)context.Data["noorder"].First().Value.Attributes["value"], Is.EqualTo(2));
        }
    }
}
