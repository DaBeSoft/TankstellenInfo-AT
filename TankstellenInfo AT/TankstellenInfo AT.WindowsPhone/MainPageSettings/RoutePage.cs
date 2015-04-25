using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using SpritpreisrechnerAtClient.Models;

namespace TankstellenInfo_AT.MainPageSettings
{
    class RoutePage : IMainPageSettings
    {
        private string _title = "In Routen Nähe";

        public string Title
        {
            get { return _title; }
            private set { _title = value; }
        }

        public Control PageSpecificControl { get; private set; }
        public async Task<IEnumerable<SpritInfo>> RefreshAction()
        {
            new MessageDialog("NOT IMPLEMENTED YET").ShowAsync();
            return new List<SpritInfo>();
        }
    }
}
