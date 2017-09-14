using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace StoreService.Models
{
    public class Entity
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}