using Newtonsoft.Json;

namespace SpritpreisrechnerAtClient.Models
{
    public class OpeningHour
    {
        [JsonProperty(PropertyName = "beginn")]
        public string Beginn { get; set; }

        [JsonProperty(PropertyName = "day")]
        public Day Day { get; set; }

        [JsonProperty(PropertyName = "end")]
        public string End { get; set; }
    }
}