using AetherFlow.Framework.Tests.Content.Models;
using AetherFlow.Framework.Tests.Example.Interfaces;

namespace AetherFlow.Framework.Tests.Example.Implementations
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
