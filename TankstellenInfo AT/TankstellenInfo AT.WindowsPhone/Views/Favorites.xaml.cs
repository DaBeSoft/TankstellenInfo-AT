using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using Newtonsoft.Json;
using TankstellenInfo_AT.Models;

namespace TankstellenInfo_AT.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Favorites : Page
    {
        public Favorites()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtonsOnBackPressed;
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("favorites.json", CreationCollisionOption.OpenIfExists);
            List<Favorite> list = JsonConvert.DeserializeObject<List<Favorite>>(await FileIO.ReadTextAsync(file)) ?? new List<Favorite>();

            foreach (var favorite in list)
            {
                var tb = new TextBlock() {Text = favorite.Name};
                stackPanel.Children.Add(tb);
            }

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (AddFavorites));
        }
        private void HardwareButtonsOnBackPressed(object sender, BackPressedEventArgs backPressedEventArgs)
        {
            HardwareButtons.BackPressed -= HardwareButtonsOnBackPressed;
            backPressedEventArgs.Handled = true;
            Frame.Navigate(typeof(MainPage));
        }
    }
}
