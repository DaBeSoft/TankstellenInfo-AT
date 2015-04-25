using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using SpritpreisrechnerAtClient.Models;

namespace TankstellenInfo_AT.MainPageSettings
{
    class FavoritesPage : IMainPageSettings
    {
        private string _title = "In Favoriten Nähe";
        private Control _pageSpecificControl = new Button { Content = "Favoriten verwalten" };

        public FavoritesPage()
        {
            _pageSpecificControl.Tapped += _pageSpecificControl_Tapped;
        }

        void _pageSpecificControl_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            new MessageDialog("NOT IMPLEMENTED YET").ShowAsync();
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
            new MessageDialog("NOT IMPLEMENTED YET").ShowAsync();
            return new List<SpritInfo>();
        }
    }
}
