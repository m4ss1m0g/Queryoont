using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Queryoont.Serialization
{
    public class JsonSerializer : IJsonSerializer
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize(object o)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = true
                }
            };

            return JsonConvert.SerializeObject(o, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
        }
    }
}