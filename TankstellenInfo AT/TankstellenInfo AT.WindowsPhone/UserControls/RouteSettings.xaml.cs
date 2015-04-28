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

    public sealed partial class RouteSettings : UserControl
    {
        public RouteSettings()
        {
            this.InitializeComponent();
        }

        public async Task<List<SpritInfo>> GetRoutePoints()
        {
            var myInfo = new List<SpritInfo>();

            if (PosBox1.Position == null || PosBox2.Position == null)
            {
                return myInfo;
            }

            var routeResult =
                await MapRouteFinder.GetDrivingRouteAsync(
                    PosBox1.Position,
                    PosBox2.Position,
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
