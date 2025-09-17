using ConnectionToSQL.Helper;
using LoginTrailTest.Admin;
using LoginTrailTest.AdminStation;
using LoginTrailTest.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LoginTrailTest
{
    public partial class StationWindow : Window
    {
        public StationWindow()
        {
            InitializeComponent();
            LoadData();
        }
        public void RefreshData()
        {
            List<Station> Station = DBHelper.GetStationsFromDatabase();
            YourData.ItemsSource = Station;
        }

        private void LoadData()
        {
            List<Station> Station = DBHelper.GetStationsFromDatabase();
            YourData.ItemsSource = Station;
        }
        private void CreateStation(object sender, RoutedEventArgs e)
        {
            CreateStation createStationWindow = new CreateStation();
            createStationWindow.ShowDialog();
            RefreshData();
        }
        private void EditStation(object sender, RoutedEventArgs e)
        {
            Station selectedStation = YourData.SelectedItem as Station;

            if (selectedStation != null)
            {
                EditStation editStationWindow = new EditStation(selectedStation);

                editStationWindow.ShowDialog();

                RefreshData();
            }
            else
            {
                MessageBox.Show("Please select a station to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteStation(object sender, RoutedEventArgs e)
        {
            if (YourData.SelectedItem != null)
            {
                Station selectedStation = (Station)YourData.SelectedItem;
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this station?", "Confirmation of deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    StationOperationsHelper.DeleteStation(selectedStation.Station_ID);
                    RefreshData();
                }
            }
            else
            {
                MessageBox.Show("Select the station to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SearchStation(object sender, RoutedEventArgs e)
        {
            SearchStation searchStationWindow = new SearchStation();
            searchStationWindow.ShowDialog();
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
