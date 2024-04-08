using RestfulApiDev.Item_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestfulApiDev
{
    public class ItemData
    {
        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("CPU model")]
        public string CpuModel { get; set; }

        [JsonPropertyName("Hard disk size")]
        public string HardDisk { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("capacity")]
        public string Capacity { get; set; }
    }
}
