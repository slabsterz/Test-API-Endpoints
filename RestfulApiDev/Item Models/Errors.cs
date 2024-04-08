using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestfulApiDev.Item_Models
{
    public class Errors
    {
        [JsonPropertyName("error")]
        public string DeletionError {  get; set; }
    }
}
