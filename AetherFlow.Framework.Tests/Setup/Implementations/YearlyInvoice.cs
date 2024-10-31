using AetherFlow.Framework.Attributes;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    [Variant("Yearly")]
    public class YearlyInvoice : Invoice
    {
        public override double GetDiscount()
        {
            return 40d;
        }
    }
}
