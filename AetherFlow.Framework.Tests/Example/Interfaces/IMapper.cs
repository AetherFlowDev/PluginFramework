namespace AetherFlow.Framework.Tests.Example.Interfaces
{
    public interface IMapper<T>
    {
        string Serialize(T record);

        T Deserialize(string data);
    }
}
