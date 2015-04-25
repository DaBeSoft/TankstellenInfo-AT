using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using SpritpreisrechnerAtClient;
using SpritpreisrechnerAtClient.Models;

namespace TankstellenInfo_AT.MainPageSettings
{
    class NearMePage : IMainPageSettings
    {
        private string _title = "In meiner Nähe";

        public string Title
        {
            get { return _title; }
            private set { _title = value; }
        }

        public Control PageSpecificControl { get; private set; }
        public async Task<IEnumerable<SpritInfo>> RefreshAction()
        {
            var a = new Geolocator();

            if (a.LocationStatus == PositionStatus.Disabled)
            {
                new MessageDialog("Gps is disabled...").ShowAsync();
                return new List<SpritInfo>();
            }

            var b = await a.GetGeopositionAsync();

            var client = new SpritPreisClient();
            return await client.GetData(b.Coordinate.Point.Position, SpritType.Super);
        }
    }
}
