using ConnectionToSQL.Helper;
using LoginTrailTest.Admin;
using LoginTrailTest.AdminRoute;
using LoginTrailTest.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LoginTrailTest
{
    public partial class SeatsWindow : Window
    {
        public SeatsWindow()
        {
            InitializeComponent();
            LoadSeats();
        }

        public void LoadSeats()
        {
            List<Seats> seats = DBHelper.GetSeatsFromDatabase();
            listView.ItemsSource = seats;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text == "")
            {
                LoadSeats();
            }
            else
            {
                List<Seats> filteredSeats = DBHelper.GetSeatsFromDatabase()
                .Where(s => s.Seat_Code.Contains(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase))
                .ToList();

                listView.ItemsSource = filteredSeats;

            }
        }

        private void CreateSeatButton_Click(object sender, RoutedEventArgs e)
        {
            CreateSeatWindow createSeatWindow = new CreateSeatWindow();
            createSeatWindow.Owner = this;
            createSeatWindow.ShowDialog();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "")
            {
                MessageBox.Show("Please enter a search term.");
            }
            else
            {
                List<Seats> filteredSeats = DBHelper.GetSeatsFromDatabase()
                    .Where(s => s.Seat_Code.Contains(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                listView.ItemsSource = filteredSeats;

            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                if (listView.SelectedItem is Seats selectedSeat)
                {
                    int seatId = selectedSeat.Seats_ID;
                    bool success = SeatsOperationHelper.DeleteSeat(seatId);

                    if (success)
                    {
                        MessageBox.Show("Location successfully deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadSeats();
                    }
                    else
                    {
                        MessageBox.Show("Error when deleting a location.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a location to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditSeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem is Seats selectedSeat)
            {
                EditSeatWindow editSeatWindow = new EditSeatWindow(selectedSeat);
                editSeatWindow.Owner = this;
                editSeatWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a location to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
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
