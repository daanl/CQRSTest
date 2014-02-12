using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infrastructure.CQRS.Serializers
{
    public class JsonNetEventSerializer : IEventSerializer
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            // Allows deserializing to the actual runtime type
            TypeNameHandling = TypeNameHandling.All,
            // In a version resilient way
            TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
        };

        public async Task<string> Serialize(object value)
        {
            return await JsonConvert.SerializeObjectAsync(value, Formatting.None, _jsonSerializerSettings);
        }

        public async Task<T> Deserialize<T>(string value)
        {
            return await JsonConvert.DeserializeObjectAsync<T>(value, _jsonSerializerSettings);
        }
    }
}
