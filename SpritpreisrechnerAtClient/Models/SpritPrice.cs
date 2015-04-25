using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace SpritpreisrechnerAtClient.Models
{
    public class SpritPrice
    {
        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        [JsonIgnore]
        internal double AmountDouble
        {
            get
            {
                try
                {
                    return Convert.ToDouble(Amount);
                }
                catch (FormatException e)
                {
                    Debug.WriteLine("FORMAT ERROR");
                    return -1;
                }
            }
        }

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

        [JsonIgnore]
        public SpritType SpritType
        {
            get { return SpritId == SpritType.Diesel.GetStringValue() ? SpritType.Diesel : SpritType.Super; }
        }
    }
}