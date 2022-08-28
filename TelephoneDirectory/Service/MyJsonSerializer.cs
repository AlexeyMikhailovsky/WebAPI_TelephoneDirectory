using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using TelephoneDirectory.Models;

namespace TelephoneDirectory.Service
{
    public static class MyJsonSerializer
    {
        private static JsonSerializerSettings settings;
        static MyJsonSerializer()
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
        /// <param name="target">object to convert</param>
        /// <returns>JSON string</returns>
        public static string MySerialize(object target){
            return JsonConvert.SerializeObject(target, settings);
        }
    }
}
