using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using SpritpreisrechnerAtClient;
using SpritpreisrechnerAtClient.Models;

namespace TankstellenInfo_AT.UserControls
{

    class MapLocationStringFriendly 
    {
        private readonly MapLocation _gp;

        public MapLocation MapLocation { get { return _gp; } }

        public MapLocationStringFriendly(MapLocation gp)
        {
            _gp = gp;
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

    public sealed partial class RouteSettings : UserControl
    {
        public RouteSettings()
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

        private Geopoint p1;
        private Geopoint p2;

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (sender.Name == "asb1")
                p1 = ((MapLocationStringFriendly)args.SelectedItem).MapLocation.Point;
            else
                p2 = ((MapLocationStringFriendly)args.SelectedItem).MapLocation.Point;

            sender.Text = args.SelectedItem.ToString();
        }

        public async Task<List<SpritInfo>> GetRoutePoints()
        {
            var myInfo = new List<SpritInfo>();

            if (p1 == null || p2 == null)
            {
                return myInfo;
            }

            var routeResult =
                await MapRouteFinder.GetDrivingRouteAsync(
                    p1,
                    p2,
                    MapRouteOptimization.Time,
                    MapRouteRestrictions.None);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {

                var client = new SpritPreisClient();



                foreach (MapRouteLeg leg in routeResult.Route.Legs)
                {
                    foreach (MapRouteManeuver maneuver in leg.Maneuvers)
                    {
                        myInfo.AddRange(await client.GetData(maneuver.StartingPoint.Position, SpritType.Super, false));

                    }
                }

                myInfo = client.SetSortAndDifference(myInfo);

            }
            else
            {
                new MessageDialog("PROBLEM").ShowAsync(); //todo
            }
            return myInfo;


        }

    }
}
