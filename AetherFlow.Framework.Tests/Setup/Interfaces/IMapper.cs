namespace AetherFlow.Framework.Tests.Setup.Interfaces
{
    public interface IMapper<T>
    {
        string Serialize(T record);

        T Deserialize(string data);
    }
}
