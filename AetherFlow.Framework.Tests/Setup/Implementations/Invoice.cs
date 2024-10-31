using AetherFlow.Framework.Tests.Setup.Interfaces;
using AetherFlow.Framework.Attributes;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    [Main]
    public class Invoice : IInvoice
    {
        public virtual double GetDiscount()
        {
            return 0;
        }

        public double GetPrice() 
        {
            var price = 100d;
            return price - ((price / 100) * GetDiscount());
        }
    }
}
