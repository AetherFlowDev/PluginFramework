using AetherFlow.Framework.Tests.Setup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
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
