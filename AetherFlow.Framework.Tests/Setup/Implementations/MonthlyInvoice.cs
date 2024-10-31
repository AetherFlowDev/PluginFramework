using AetherFlow.Framework.Attributes;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    [Variant("Monthly")]
    public class MonthlyInvoice : Invoice
    {
        public override double GetDiscount()
        {
            return 10d;
        }
    }
}
