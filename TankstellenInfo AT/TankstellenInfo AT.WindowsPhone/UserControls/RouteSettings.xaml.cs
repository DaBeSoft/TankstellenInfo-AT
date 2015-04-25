using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using Windows.UI.Xaml.Documents;
using SpritpreisrechnerAtClient;
using SpritpreisrechnerAtClient.Models;

namespace TankstellenInfo_AT.UserControls
{
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

        private static async Task<object> GetSuggestions(string text)
        {
            var result = await MapLocationFinder.FindLocationsAsync(text, new Geopoint(new BasicGeoposition() { Latitude = 48.209206, Longitude = 16.372778 }), 7);
            //return result.Locations.Select(l => string.Format("{0} {1}, {2} {3}", l.Address.Street, l.Address.StreetNumber, l.Address.PostCode, l.Address.Town)).ToList();
            return result.Locations.Select(l => l.Point).ToList();
        }

        private Geopoint p1;
        private Geopoint p2;

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (sender.Name == "asb1")
                p1 = (Geopoint)args.SelectedItem;
            else
            {
                p2 = (Geopoint)args.SelectedItem;
            }
            sender.Text = args.SelectedItem.ToString();
        }

        public async Task<List<SpritInfo>> GetRoutePoints()
        {
            var myInfo = new List<SpritInfo>();

            if (p1 == null || p2 == null)
            {
                return myInfo;
            }

            MapRouteFinderResult routeResult =
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


                //foreach (var pos in routeResult.Route.Path.Positions)
                //{
                //    myInfo.AddRange(await client.GetData(pos, SpritType.Super));
                //}

                myInfo = client.SetSortAndDifference(myInfo);

            }
            else
            {
                new MessageDialog("PROBLEM").ShowAsync();
            }
            return myInfo;


        }

    }
}
