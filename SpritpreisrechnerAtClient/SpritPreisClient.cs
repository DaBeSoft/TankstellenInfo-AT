using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Web.Http;
using Newtonsoft.Json;
using SpritpreisrechnerAtClient.Models;

namespace SpritpreisrechnerAtClient
{
    public class SpritPreisClient
    {
        private readonly Uri _serviceUrl = new Uri("http://www.spritpreisrechner.at/ts/GasStationServlet");
        private const string ContentTemplate = "[\"\",\"{0}\",{1},{2},{3},{4}]";

        public async Task<List<SpritInfo>> GetData(BasicGeoposition position, SpritType type, bool sort = true)
        {
            var bottomLeft = new BasicGeoposition() { Longitude = position.Longitude - 0.04 /*16,316736214513785*/, Latitude = position.Latitude - 0.04 /*48,07149781819904*/ };
            var topRight = new BasicGeoposition() { Longitude = position.Longitude + 0.04/*16,396644585490413 */, Latitude = position.Latitude + 0.04/*48,09443362514294*/ };

            var client = new HttpClient();

            var message = new HttpRequestMessage(HttpMethod.Post, _serviceUrl)
            {
                Content = new HttpFormUrlEncodedContent(new[] { new KeyValuePair<string, string>
                    ("data", string.Format(ContentTemplate, type.GetStringValue(), bottomLeft.Longitude, bottomLeft.Latitude, topRight.Longitude, topRight.Latitude)) })
            };

            var result = await client.SendRequestAsync(message);

            var json = result.Content.ToString();

            var list = JsonConvert.DeserializeObject<List<SpritInfo>>(json);
            list = list.Where(l => l.SpritPrice[0].AmountDouble != -1).ToList();
            if (sort) list = SetSortAndDifference(list);

            return list;
        }

        public List<SpritInfo> SetSortAndDifference(List<SpritInfo> list)
        {
            list = list.Distinct().OrderBy(l => l.SpritPrice[0].AmountDouble).ToList();

            //todo clear double entries!!!


            var cheapest = list[0].SpritPrice[0].AmountDouble;
            list[0].SortPosition = "1.";
            list[0].PriceDifference = "0,00€";

            for (int i = 1; i < list.Count; i++)
            {
                list[i].SortPosition = (i + 1) + ".";
                list[i].PriceDifference = "(+" + Math.Round(list[i].SpritPrice[0].AmountDouble - cheapest, 3) + "€)";
            }
            return list;

        }
    }
}
