using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Contraindicator.Models.Nodes
{
    [JsonObject("Product")]
    public class Product
    {
        [JsonProperty("ProductId")]
        public string ProductId { get; set; } = Guid.NewGuid().ToString("N");

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Properties")]
        public Dictionary<string,string> Properties { get; set; }
    }
}
