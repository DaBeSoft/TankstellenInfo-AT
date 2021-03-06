﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using TankstellenInfo_AT.MainPageSettings;
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
            lblTitle.Text = App.MainPageSettings.Title;
            spHeader.Children.Clear();
            if (App.MainPageSettings.PageSpecificControl != null) spHeader.Children.Add(App.MainPageSettings.PageSpecificControl);
            spHeader.Children.Add(new Rectangle() { Fill = Foreground, Height = 80 });

            if (App.MainPageSettings.GetType() == typeof (NearMePage))
                abtnPosition.IsEnabled = false;
            if (App.MainPageSettings.GetType() == typeof(RoutePage))
                abtnDirection.IsEnabled = false;
            if (App.MainPageSettings.GetType() == typeof(FavoritesPage))
                abtnFavorites.IsEnabled = false;


            Button_Click(null, null);
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            btnRefresh.IsEnabled = false;
            await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();
            ContentPanel.Children.Clear();

            var results = await App.MainPageSettings.RefreshAction();

            foreach (var result in results)
            {
                var test = new GasInfoControl(result);
                ContentPanel.Children.Add(test);
            }

            btnRefresh.IsEnabled = true;
            await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ProgressIndicator.HideAsync();
        }

        private void abtnFavorites_Click(object sender, RoutedEventArgs e)
        {
            App.MainPageSettings = new FavoritesPage();
            Frame.Navigate(typeof(MainPage));
        }

        private void abtnDirection_Click(object sender, RoutedEventArgs e)
        {
            App.MainPageSettings = new RoutePage();
            Frame.Navigate(typeof(MainPage));

        }

        private void abtnSettings_Click(object sender, RoutedEventArgs e)
        {
            spHeader.Children.Clear();
            Frame.Navigate(typeof(SettingsPage));
        }

        private void abtnPosition_Click(object sender, RoutedEventArgs e)
        {
            App.MainPageSettings = new NearMePage();
            Frame.Navigate(typeof(MainPage));
        }

        private void CommandBar_Opened(object sender, object e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            HeaderRow.Height = new GridLength(spHeader.ActualHeight);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            spHeader.Children.Clear();

        }


    }
}
