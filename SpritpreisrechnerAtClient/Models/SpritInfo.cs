using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpritpreisrechnerAtClient.Models
{
    public class SpritInfo
    {
        [JsonProperty(PropertyName = "kredit")]
        public bool Kredit { get; set; }

        [JsonProperty(PropertyName = "self")]
        public bool Self { get; set; }

        [JsonProperty(PropertyName = "spritPrice")]
        public List<SpritPrice> SpritPrice { get; set; }

        [JsonProperty(PropertyName = "automat")]
        public bool Automat { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "open")]
        public bool Open { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }

        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty(PropertyName = "errorItems")]
        public List<ErrorItem> ErrorItems { get; set; }

        [JsonProperty(PropertyName = "priceSearchDisabled")]
        public bool PriceSearchDisabled { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "payMethod")]
        public string PayMethod { get; set; }

        [JsonProperty(PropertyName = "mail")]
        public string Mail { get; set; }

        [JsonProperty(PropertyName = "gasStationName")]
        public string GasStationName { get; set; }

        [JsonProperty(PropertyName = "fax")]
        public string Fax { get; set; }

        [JsonProperty(PropertyName = "clubCard")]
        public string ClubCard { get; set; }

        [JsonProperty(PropertyName = "openingHours")]
        public List<OpeningHour> OpeningHours { get; set; }

        [JsonProperty(PropertyName = "access")]
        public string Access { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "serviceText")]
        public string ServiceText { get; set; }

        [JsonProperty(PropertyName = "maestro")]
        public bool Maestro { get; set; }

        [JsonProperty(PropertyName = "companionship")]
        public bool Companionship { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "club")]
        public bool Club { get; set; }

        [JsonProperty(PropertyName = "errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty(PropertyName = "service")]
        public bool Service { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "bar")]
        public bool Bar { get; set; }

        [JsonProperty(PropertyName = "telephone")]
        public string Telephone { get; set; }

        [JsonIgnore]
        public string PriceAndType
        {
            get { return string.Format("{0} ({1})", SpritPrice[0].Amount, SpritPrice[0].SpritType); }
        }

        [JsonIgnore]
        public string CityAndPostalCode
        {
            get { return string.Format("{0} {1}", PostalCode, City); }
        }

        [JsonIgnore]
        public string PriceDifference { get; set; }
        [JsonIgnore]
        public string SortPosition { get; set; }
    }


    public class ErrorItem
    {
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("msgText")]
        public string MsgText { get; set; }
    }
}

