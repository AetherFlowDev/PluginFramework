using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using AetherFlow.Framework.Interfaces;

namespace AetherFlow.Framework.Helpers
{
    public class JsonContractSerializer : IJsonSerializer
    {
        public string Serialize(object obj)
        {
            if (obj == null) return null;

            var serializer = new DataContractJsonSerializer(obj.GetType());
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, obj);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public T Deserialize<T>(string input) where T : new()
        {
            if (string.IsNullOrEmpty(input)) return new T();

            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(input)))
            {
                T obj = (T)serializer.ReadObject(memoryStream);
                return obj;
            }
        }
    }
}
