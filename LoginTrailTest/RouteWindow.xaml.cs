using ConnectionToSQL.Helper;
using LoginTrailTest.Admin;
using LoginTrailTest.AdminPanel;
using LoginTrailTest.AdminRoute;
using LoginTrailTest.Helper;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace LoginTrailTest
{
    public partial class RouteWindow : Window
    {
        public RouteWindow()
        {
            InitializeComponent();
            LoadData();
        }
        public void RefreshData()
        {
            List<Route> routes = DBHelper.GetRoutesFromDatabase();
            YourData.ItemsSource = routes;
        }
        private void LoadData()
        {
            List<Route> routes = DBHelper.GetRoutesFromDatabase();
            YourData.ItemsSource = routes;
        }

        private void CreateButtonRoute(object sender, RoutedEventArgs e)
        {
            CreateRoute createRouteWindow = new CreateRoute();
            createRouteWindow.ShowDialog();
        }

        private void EditButtonRoute(object sender, RoutedEventArgs e)
        {
            if (YourData.SelectedItem != null && YourData.SelectedItem is Route selectedRoute)
            {
                EditRoute editRouteWindow = new EditRoute(selectedRoute);
                editRouteWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select a route to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButtonRoute(object sender, RoutedEventArgs e)
        {
            if (YourData.SelectedItem != null)
            {
                Route selectedRoute = (Route)YourData.SelectedItem;
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this route??", "Confirmation of deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    RouteOperationsHelper.DeleteRoute(selectedRoute.Route_ID);
                    RefreshData();
                }
            }
            else
            {
                MessageBox.Show("Select a route to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchButtonRoute(object sender, RoutedEventArgs e)
        {
            SearchRoute searchRouteWindow = new SearchRoute();
            searchRouteWindow.ShowDialog();
        }
        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            TrainWindow trainWindow = new TrainWindow();
            trainWindow.Show();
            this.Close();
        }
        private void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            SeatsWindow seatsWindow = new SeatsWindow();
            seatsWindow.Show();
            this.Close();
        }
        private void RouteButton_Click(object sender, RoutedEventArgs e)
        {
            RouteWindow routeWindow = new RouteWindow();
            routeWindow.Show();
            this.Close();
        }
        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleWindow scheduleWindow = new ScheduleWindow();
            scheduleWindow.Show();
            this.Close();
        }
        private void MapMarkerPath_Click(object sender, RoutedEventArgs e)
        {
            StationWindow stationWindow = new StationWindow();
            stationWindow.Show();
            this.Close();
        }
        private void TicketAccount_Click(object sender, RoutedEventArgs e)
        {
            TicketWindow ticketWindow = new TicketWindow();
            ticketWindow.Show();
            this.Close();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                this.DragMove();
        }
        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
