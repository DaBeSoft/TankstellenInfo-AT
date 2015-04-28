using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;

namespace TankstellenInfo_AT.UserControls
{

    public class MapLocationStringFriendly
    {
        private readonly MapLocation _gp;

        [JsonIgnore]
        public MapLocation MapLocation { get { return _gp; } }

        public Geopoint Geopoint { get; set; }

        public string ToStringValue { get; set; }

        public MapLocationStringFriendly(MapLocation gp)
        {
            _gp = gp;
            Geopoint = gp.Point;
            ToStringValue = ToString();
        }

        public MapLocationStringFriendly()
        {
            
        }

        public override string ToString()
        {
            var retval = "";


            if (!string.IsNullOrEmpty(_gp.DisplayName))
                return _gp.DisplayName;


            if (!string.IsNullOrEmpty(_gp.Address.Street))
            {
                retval += _gp.Address.Street;
                if (!string.IsNullOrEmpty(_gp.Address.StreetNumber))
                {
                    retval += " " + _gp.Address.StreetNumber;
                }
                retval += ", ";
            }

            if (!string.IsNullOrEmpty(_gp.Address.PostCode))
            {
                retval += _gp.Address.PostCode;
                if (!string.IsNullOrEmpty(_gp.Address.Town))
                {
                    retval += " " + _gp.Address.Town;
                }
            }

            return retval;

        }
    }

    public sealed partial class GeolocationAutocompleteBox : UserControl
    {
        public Geopoint Position;
        public MapLocationStringFriendly MapLocationStringFriendly { get; set; }
        public GeolocationAutocompleteBox()
        {
            this.InitializeComponent();
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                // You can set a threshold when to start looking for suggestions
                if (sender.Text.Length > 3)
                {
                    sender.ItemsSource = await GetSuggestions(sender.Text);

                }
                else
                {
                    sender.ItemsSource = new List<String>();
                }
            }
        }

        private static async Task<IEnumerable<MapLocationStringFriendly>> GetSuggestions(string text)
        {
            var result = await MapLocationFinder.FindLocationsAsync(text, new Geopoint(new BasicGeoposition() { Latitude = 48.209206, Longitude = 16.372778 }), 7);

            var retVal = result.Locations.Select(l => new MapLocationStringFriendly(l)).ToList();

            return retVal;
        }
        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Position = ((MapLocationStringFriendly)args.SelectedItem).MapLocation.Point;
            MapLocationStringFriendly = ((MapLocationStringFriendly) args.SelectedItem);
            sender.Text = args.SelectedItem.ToString();
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var gps = new Geolocator();
            //gps.DesiredAccuracy = PositionAccuracy.High;
            var a = await gps.GetGeopositionAsync();

            Position = a.Coordinate.Point;

            var result = await MapLocationFinder.FindLocationsAtAsync(a.Coordinate.Point);
            var retVal = result.Locations.Select(l => new MapLocationStringFriendly(l)).ToList();

            MapLocationStringFriendly = retVal[0];

            asb1.Text = retVal[0].ToString();
        }
    }
}
