using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestfulApiDev.Item_Models
{
    public class Item 
    {
        [JsonPropertyName("id")] 
        public string Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("data")]
        public ItemData ItemData { get; set; }

        public Item()
        {
            ItemData = new ItemData();
        }

    }
}
