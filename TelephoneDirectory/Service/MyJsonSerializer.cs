using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using TelephoneDirectory.Models;

namespace TelephoneDirectory.Service
{
    public class MyJsonSerializer
    {
        private JsonSerializerSettings settings;
        private string json;
        public MyJsonSerializer()
        {
            settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Newtonsoft.Json.Formatting.Indented
            };
        }
        /// <summary>
        /// Converts object to JSON string
        /// </summary>
        /// <param name="o">object to convert</param>
        /// <returns>JSON string</returns>
        public string MySerialize(object o){
            json = JsonConvert.SerializeObject(o, settings);
            return json;
        }

    }
}
