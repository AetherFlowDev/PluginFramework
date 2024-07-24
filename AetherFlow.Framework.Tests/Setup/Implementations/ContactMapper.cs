using AetherFlow.Framework.Tests.Setup.Interfaces;
using AetherFlow.Framework.Tests.Setup.Models;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    public class ContactMapper : IMapper<Contact>
    {
        public string Serialize(Contact record)
        {
            return "Contact";
        }

        public Contact Deserialize(string data)
        {
            return new Contact();
        }
    }
}
