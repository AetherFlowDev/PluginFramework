namespace AetherFlow.Framework.Interfaces
{
    public interface IJsonSerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string input) where T : new();
    }
}
