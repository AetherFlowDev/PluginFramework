using AetherFlow.Framework.Testing;
using AetherFlow.Framework.Testing.Extensions;
using AetherFlow.Framework.Tests.Setup.Implementations;
using AetherFlow.Framework.Tests.Setup.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Framework.Tests.UnitTests.DataverseContainerTests
{
    [TestFixture]
    public class CheckInvoiceGetsCorrectInstance : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run()
        {
            this.UseContainer();
            this.UseAssembly(GetType().Assembly);
            this.LoadDependencies("AetherFlow.Framework.Tests.Setup.Interfaces");
            RunSpecification();
        }

        // ACT variables
        private IInvoice Invoice;
        private IInvoice MonthlyInvoice;
        private IInvoice YearlyInvoice;

        public override void Act()
        {
            var container = this.GetContainer();
            Invoice = container.Get<IInvoice>();
            MonthlyInvoice = container.Get<MonthlyInvoice>();
            YearlyInvoice = container.Get<YearlyInvoice>();
        }

        [Test]
        public void EnsureNoExceptionThrown()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void EnsureInvoiceValuesCorrect()
        {
            Assert.That(Invoice.GetPrice(), Is.EqualTo(100d));
            Assert.That(MonthlyInvoice.GetPrice(), Is.EqualTo(90d));
            Assert.That(YearlyInvoice.GetPrice(), Is.EqualTo(60d));
        }
    }
}