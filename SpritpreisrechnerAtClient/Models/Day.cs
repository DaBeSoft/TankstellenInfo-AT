using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpritpreisrechnerAtClient.Models
{
    public class Day
    {
        [JsonProperty(PropertyName = "dayLabel")]
        public string DayLabel { get; set; }

        [JsonProperty(PropertyName = "order")]
        public int Order { get; set; }

        [JsonProperty(PropertyName = "errorItems")]
        public List<object> ErrorItems { get; set; }

        [JsonProperty(PropertyName = "errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty(PropertyName = "day")]
        public string DayShort { get; set; }
    }
}