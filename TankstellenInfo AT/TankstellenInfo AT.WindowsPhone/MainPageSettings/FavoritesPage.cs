using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using SpritpreisrechnerAtClient;
using SpritpreisrechnerAtClient.Models;
using TankstellenInfo_AT.Models;
using TankstellenInfo_AT.Views;

namespace TankstellenInfo_AT.MainPageSettings
{
    class FavoritesPage : IMainPageSettings
    {
        private string _title = "In Favoriten Nähe";

        private Control _pageSpecificControl = new Button
        {
            Content = "Favoriten verwalten",
            Margin = new Thickness(0, 0, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        public FavoritesPage()
        {
            _pageSpecificControl.Tapped += _pageSpecificControl_Tapped;
        }

        void _pageSpecificControl_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            App.Frame.Navigate(typeof (Favorites));
        }

        public string Title
        {
            get { return _title; }
            private set { _title = value; }
        }

        public Control PageSpecificControl
        {
            get { return _pageSpecificControl; }
            private set { _pageSpecificControl = value; }
        }

        public async Task<IEnumerable<SpritInfo>> RefreshAction()
        {
            SpritPreisClient client = new SpritPreisClient();



            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("favorites.json", CreationCollisionOption.OpenIfExists);


            List<Favorite> list = JsonConvert.DeserializeObject<List<Favorite>>(await FileIO.ReadTextAsync(file)) ?? new List<Favorite>();


            var retVal = new List<SpritInfo>();

            foreach (var favorite in list)
            {
                retVal.AddRange(await client.GetData(new BasicGeoposition(){Latitude = favorite.Latitude, Longitude = favorite.Longitude}, SpritType.Super));
            }

            retVal = client.SetSortAndDifference(retVal);

            return retVal;
        }
    }
}
