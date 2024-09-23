using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    public class YearlyInvoice : Invoice
    {
        public override double GetDiscount()
        {
            return 40d;
        }
    }
}
