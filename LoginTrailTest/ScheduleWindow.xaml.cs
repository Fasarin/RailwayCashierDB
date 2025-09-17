using ConnectionToSQL.Helper;
using LoginTrailTest.Admin;
using LoginTrailTest.AdminRoute;
using LoginTrailTest.AdminSchedule;
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
    public partial class ScheduleWindow : Window
    {
        public ScheduleWindow()
        {
            InitializeComponent();
            LoadData();
        }

        public void RefreshData()
        {
            List<Schedule> Schedule = DBHelper.GetSchedulesFromDatabase();
            YourData.ItemsSource = Schedule;
        }

        private void LoadData()
        {
            List<Schedule> Schedule = DBHelper.GetSchedulesFromDatabase();
            YourData.ItemsSource = Schedule;
        }

        private void CreateSсhedule(object sender, RoutedEventArgs e)
        {
            CreateSchedule createScheduleWindow = new CreateSchedule();
            createScheduleWindow.ShowDialog();
        }

        private void EditSchedule(object sender, RoutedEventArgs e)
        {
            Schedule selectedSchedule = YourData.SelectedItem as Schedule;

            if (selectedSchedule != null)
            {
                EditSchedule editScheduleWindow = new EditSchedule(selectedSchedule);

                editScheduleWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a schedule to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteSchedule(object sender, RoutedEventArgs e)
        {
            if (YourData.SelectedItem != null)
            {
                Schedule selectedSchedule = (Schedule)YourData.SelectedItem;
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this schedule?", "Confirmation of deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ScheduleOperationsHelper.DeleteSchedule(selectedSchedule.Schedule_ID);
                    RefreshData();
                }
            }
            else
            {
                MessageBox.Show("Select a schedule for deletion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchSchedule(object sender, RoutedEventArgs e)
        {
            SearchSchedule searchScheduleWindow = new SearchSchedule();
            searchScheduleWindow.ShowDialog();
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
