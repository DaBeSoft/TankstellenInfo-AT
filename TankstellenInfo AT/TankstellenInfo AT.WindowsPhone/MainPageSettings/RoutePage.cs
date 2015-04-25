using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using SpritpreisrechnerAtClient.Models;
using TankstellenInfo_AT.UserControls;

namespace TankstellenInfo_AT.MainPageSettings
{
    class RoutePage : IMainPageSettings
    {
        private string _title = "In Routen Nähe";
        private readonly RouteSettings _pageSpecificControl = new RouteSettings();

        public string Title
        {
            get { return _title; }
            private set { _title = value; }
        }

        public Control PageSpecificControl
        {
            get { return _pageSpecificControl; }
        }

        public async Task<IEnumerable<SpritInfo>> RefreshAction()
        {
            return await _pageSpecificControl.GetRoutePoints();
        }
    }
}
