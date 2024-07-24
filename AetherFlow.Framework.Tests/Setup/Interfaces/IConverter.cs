using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherFlow.Framework.Tests.Setup.Interfaces
{
    public interface IConverter<T, TO>
    {
        TO From(T input);
        T To(TO input);
    }
}
