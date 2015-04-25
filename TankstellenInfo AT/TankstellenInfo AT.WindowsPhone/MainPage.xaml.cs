using System;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using SpritpreisrechnerAtClient;
using SpritpreisrechnerAtClient.Models;
using TankstellenInfo_AT.UserControls;

namespace TankstellenInfo_AT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Button_Click(null, null);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            btnRefresh.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
            StackPanel.Children.Clear();


            var a = new Geolocator();
            var b = await a.GetGeopositionAsync();
            
            var client = new SpritPreisClient();
            var results = await client.GetData(b.Coordinate.Point.Position, SpritType.Super);

            foreach (var result in results)
            {
                var test = new GasInfoControl(result);
                StackPanel.Children.Add(test);
            }

            btnRefresh.IsEnabled = true;
            progressBar.Visibility = Visibility.Collapsed;

        }

        private void abtnFavorites_Click(object sender, RoutedEventArgs e)
        {
            new MessageDialog("Tankstellen in der Nähe von favorisierten Orten ist noch nicht implementiert!").ShowAsync();
        }

        private void abtnDirection_Click(object sender, RoutedEventArgs e)
        {
            new MessageDialog("Tankstellen entlang einer Route ist noch nicht implementiert!").ShowAsync();

        }

        private void abtnSettings_Click(object sender, RoutedEventArgs e)
        {
            new MessageDialog("Einstellungen ist noch nicht implementiert!").ShowAsync();

        }
    }
}
