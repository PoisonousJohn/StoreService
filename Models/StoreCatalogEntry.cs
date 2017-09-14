using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace StoreService.Models
{
    public class StoreCatalogEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required]
        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("price")]
        public float Price { get; set; }

        [Required]
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}