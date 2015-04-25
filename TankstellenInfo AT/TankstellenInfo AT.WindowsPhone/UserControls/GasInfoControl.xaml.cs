using System;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SpritpreisrechnerAtClient.Models;

namespace TankstellenInfo_AT.UserControls
{
    public sealed partial class GasInfoControl : UserControl
    {
        private readonly SpritInfo _si;

        public GasInfoControl(SpritInfo si)
        {
            _si = si;
            this.InitializeComponent();

            lblName.Text = si.GasStationName;
            lblPrice.Text = si.SpritPrice[0].Amount + "€";
            lblGasType.Text = si.SpritPrice[0].SpritType.ToString();


            lblAddress1.Text = si.Address;
            lblAddress2.Text = si.CityAndPostalCode;

            lblPos.Text = si.SortPosition;
            lblPriceDiference.Text = si.PriceDifference;

            lblDistance.Text = "Entfernung: " +_si.Distance + "km";
        }

        private void btnNav_Click(object sender, RoutedEventArgs e)
        {
            var driveToUri = new Uri(string.Format(
                 "ms-drive-to:?destination.latitude={0}&destination.longitude={1}&destination.name={2}",
                 _si.Latitude,
                 _si.Longitude,
                 _si.GasStationName));

            Launcher.LaunchUriAsync(driveToUri);
        }

        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            new MessageDialog("NOT IMPLEMENTED YET").ShowAsync();
        }

        private void btnMap_Click(object sender, RoutedEventArgs e)
        {
            var mapUri = new Uri(string.Format("bingmaps:?collection=point.{0}_{1}_{2}", _si.Latitude, _si.Longitude, _si.GasStationName));

            Launcher.LaunchUriAsync(mapUri);
        }



    }
}
