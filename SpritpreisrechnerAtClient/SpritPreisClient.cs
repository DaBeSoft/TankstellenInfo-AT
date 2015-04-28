using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
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
            var bottomLeft = new BasicGeoposition()
            {
                Longitude = position.Longitude - 0.04 /*16,316736214513785*/,
                Latitude = position.Latitude - 0.01 /*48,07149781819904*/
            };
            var topRight = new BasicGeoposition()
            {
                Longitude = position.Longitude + 0.04 /*16,396644585490413 */,
                Latitude = position.Latitude + 0.01 /*48,09443362514294*/
            };

            var box =
                GeoCalculator.GetBoundingBox(new GeoCalculator.MapPoint()
                {
                    Latitude = position.Latitude,
                    Longitude = position.Longitude
                }, 10);


            var client = new HttpClient();

            var message = new HttpRequestMessage(HttpMethod.Post, _serviceUrl)
            {
                Content = new HttpFormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>
                //        ("data",
                //            "[\"\",\"DIE\",16.414713814517117,48.24282285698656,16.49462218548476,48.26568218805452]")
                //})
                ("data", string.Format(ContentTemplate, type.GetStringValue(), box.MinPoint.Longitude.ToString(new CultureInfo("en-US")), box.MinPoint.Latitude.ToString(new CultureInfo("en-US")), box.MaxPoint.Longitude.ToString(new CultureInfo("en-US")), box.MaxPoint.Latitude.ToString(new CultureInfo("en-US")))) })
                //("data", string.Format(ContentTemplate, type.GetStringValue(), bottomLeft.Longitude, bottomLeft.Latitude, topRight.Longitude, topRight.Latitude)) })
            };

            var result = await client.SendRequestAsync(message);

            var json = result.Content.ToString();

            var list = JsonConvert.DeserializeObject<List<SpritInfo>>(json);

            if (list[0].ErrorItems.Count != 0)
            {
                await new MessageDialog(list[0].ErrorItems[0].MsgText).ShowAsync();
                return new List<SpritInfo>();
            }

            list = list.Where(l => l.SpritPrice[0].Amount.HasValue).ToList();
            if (sort) list = SetSortAndDifference(list);

            return list;
        }

        public List<SpritInfo> SetSortAndDifference(List<SpritInfo> list)
        {
            if (list == null || list.Count == 0)
                return list;

            list = list.Distinct().OrderBy(l => l.SpritPrice[0].Amount).ToList();

            //todo clear double entries!!!
            list = CheckDoubles(0, list);

            var cheapest = list[0].SpritPrice[0].Amount.Value;
            list[0].SortPosition = "1.";
            list[0].PriceDifference = "0,00€";

            for (int i = 1; i < list.Count; i++)
            {
                list[i].SortPosition = (i + 1) + ".";
                list[i].PriceDifference = "(+" + Math.Round(list[i].SpritPrice[0].Amount.Value - cheapest, 3) + "€)";
            }
            return list;

        }

        private static List<SpritInfo> CheckDoubles(int i, List<SpritInfo> list)
        {
            for (; i < list.Count; i++)
            {
                for (var j = i + 1; j < list.Count; j++)
                {
                    if (list[i].GasStationName == list[j].GasStationName && list[i].Address == list[j].Address &&
                        list[i].CityAndPostalCode == list[j].CityAndPostalCode)
                    {
                        list.RemoveAt(j);
                        return CheckDoubles(i, list);
                    }
                }
            }
            return list;
        }

        public static double Calculate(double sLatitude, double sLongitude, double eLatitude,
            double eLongitude)
        {
            var sLatitudeRadians = sLatitude*(Math.PI/180.0);
            var sLongitudeRadians = sLongitude*(Math.PI/180.0);
            var eLatitudeRadians = eLatitude*(Math.PI/180.0);
            var eLongitudeRadians = eLongitude*(Math.PI/180.0);

            var dLongitude = eLongitudeRadians - sLongitudeRadians;
            var dLatitude = eLatitudeRadians - sLatitudeRadians;

            var result1 = Math.Pow(Math.Sin(dLatitude/2.0), 2.0) +
                          Math.Cos(sLatitudeRadians)*Math.Cos(eLatitudeRadians)*
                          Math.Pow(Math.Sin(dLongitude/2.0), 2.0);

            // Using 3956 as the number of miles around the earth
            var result2 = 3956.0*2.0*
                          Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return result2;

        }

        //public static double Calculate(BasicGeoposition position, double kilometersToAdd)
        //{
        //    Geocircle a = new Geocircle(position, 10);
            

        //    var sLatitudeRadians = position.Latitude * (Math.PI / 180.0);
        //    var sLongitudeRadians = position.Longitude * (Math.PI / 180.0);
           

        //    var dLongitude = eLongitudeRadians - sLongitudeRadians;
        //    var dLatitude = eLatitudeRadians - sLatitudeRadians;

        //    var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
        //                  Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) *
        //                  Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

        //    // Using 3956 as the number of miles around the earth
        //    var result2 = 3956.0 * 2.0 *
        //                  Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));


        //    var eLatitudeRadians = eLatitude * (Math.PI / 180.0);
        //    var eLongitudeRadians = eLongitude * (Math.PI / 180.0);

        //    return result2;

        //}
    }
}