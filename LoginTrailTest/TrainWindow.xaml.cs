using ConnectionToSQL.Helper;
using LoginTrailTest.Admin;
using LoginTrailTest.AdminPanel;
using LoginTrailTest.Helper;
using System;
using System.Collections.Generic;
using System.Windows;

namespace LoginTrailTest
{
    public partial class TrainWindow : Window
    {
        public TrainWindow()
        {
            InitializeComponent();
            List<Train> trains = DBHelper.GetTrainsFromDatabase();
            YourData.ItemsSource = trains;
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTrainWindow createTrainWindow = new CreateTrainWindow();
            createTrainWindow.ShowDialog();

            List<Train> trains = DBHelper.GetTrainsFromDatabase();
            YourData.ItemsSource = trains;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (YourData.SelectedItem != null)
            {
                Train selectedTrain = (Train)YourData.SelectedItem;

                EditTrainWindow editTrainWindow = new EditTrainWindow(selectedTrain);
                editTrainWindow.ShowDialog();

                List<Train> trains = DBHelper.GetTrainsFromDatabase();
                YourData.ItemsSource = trains;
            }
            else
            {
                MessageBox.Show("Select a train to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (YourData.SelectedItem != null)
            {
                Train selectedTrain = (Train)YourData.SelectedItem;
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this train?", "Confirmation of deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    TrainOperationsHelper.DeleteTrain(selectedTrain.Train_ID);
                    List<Train> trains = DBHelper.GetTrainsFromDatabase();
                    YourData.ItemsSource = trains;
                }
            }
            else
            {
                MessageBox.Show("Select the train to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTrainWindow searchTrainWindow = new SearchTrainWindow();
            searchTrainWindow.SearchCompleted += SearchTrainWindow_SearchCompleted;
            searchTrainWindow.ShowDialog();
        }

        private void SearchTrainWindow_SearchCompleted(object sender, EventArgs e)
        {
            List<Train> trains = DBHelper.GetTrainsFromDatabase();
            YourData.ItemsSource = trains;
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
