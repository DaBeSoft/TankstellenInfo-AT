using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpritpreisrechnerAtClient.Models
{
    public class SpritPrice
    {
        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        [JsonProperty(PropertyName = "datAnounce")]
        public string DatAnounce { get; set; }

        [JsonProperty(PropertyName = "errorItems")]
        public List<object> ErrorItems { get; set; }

        [JsonProperty(PropertyName = "errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty(PropertyName = "datValid")]
        public object DatValid { get; set; }

        [JsonProperty(PropertyName = "spritId")]
        public string SpritId { get; set; }
    }
}