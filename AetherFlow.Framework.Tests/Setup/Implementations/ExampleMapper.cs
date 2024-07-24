using AetherFlow.Framework.Tests.Setup.Interfaces;

namespace AetherFlow.Framework.Tests.Setup.Implementations
{
    public class ExampleMapper : IMapper<InstanceWithNoConstructor>
    {
        public string Serialize(InstanceWithNoConstructor record)
        {
            return "InstanceWithNoConstructor";
        }

        public InstanceWithNoConstructor Deserialize(string data)
        {
            return new InstanceWithNoConstructor();
        }
    }
}
