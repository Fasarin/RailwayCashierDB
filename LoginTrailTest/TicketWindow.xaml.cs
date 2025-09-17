using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConnectionToSQL.Helper;
using LoginTrailTest.Admin;
using iTextSharp.text;

namespace LoginTrailTest
{
    public partial class TicketWindow : Window
    {
        List<TicketSale> TicketSales;

        public TicketWindow()
        {
            InitializeComponent();
            TicketSales = DBHelper.GetTicketSalesFromDatabase();
            YourData.ItemsSource = TicketSales;
        }

        private void SearchSchedule(object sender, RoutedEventArgs e)
        {
            List<TicketSale> filteredTicketSales = TicketSales;

            // Фільтрація за датою, якщо вибрано
            if (DatePicker.SelectedDate.HasValue)
            {
                filteredTicketSales = filteredTicketSales.Where(ts => ts.Departure_Date.Date == DatePicker.SelectedDate.Value.Date).ToList();
            }

            // Фільтрація за словами в полі пошуку
            string searchText = SearchTextBox.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredTicketSales = filteredTicketSales.Where(ts =>
                    ts.Ticket_Code.ToLower().Contains(searchText) ||
                    ts.Train_Number.ToLower().Contains(searchText) ||
                    ts.Seat_Type.ToLower().Contains(searchText)
                ).ToList();
            }

            // Перевірка, чи є результати пошуку
            if (filteredTicketSales.Any())
            {
                YourData.ItemsSource = filteredTicketSales;
            }
            else
            {
                MessageBox.Show("No results found.", "Search results", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private Dictionary<string, int> CalculateTicketsPerTrain()
        {
            Dictionary<string, int> ticketsPerTrain = new Dictionary<string, int>();

            foreach (var ticketSale in TicketSales)
            {
                if (ticketsPerTrain.ContainsKey(ticketSale.Train_Number))
                    ticketsPerTrain[ticketSale.Train_Number]++;
                else
                    ticketsPerTrain[ticketSale.Train_Number] = 1;
            }

            return ticketsPerTrain;
        }

        private Dictionary<string, decimal> CalculateMoneyPerTrain()
        {
            Dictionary<string, decimal> moneyPerTrain = new Dictionary<string, decimal>();

            foreach (var ticketSale in TicketSales)
            {
                if (moneyPerTrain.ContainsKey(ticketSale.Train_Number))
                    moneyPerTrain[ticketSale.Train_Number] += ticketSale.Price;
                else
                    moneyPerTrain[ticketSale.Train_Number] = ticketSale.Price;
            }

            return moneyPerTrain;
        }

        private Dictionary<string, int> CalculateTicketsPerMonth()
        {
            Dictionary<string, int> ticketsPerMonth = new Dictionary<string, int>();

            foreach (var ticketSale in TicketSales)
            {
                string monthYear = ticketSale.Departure_Date.ToString("MM/yyyy");
                if (ticketsPerMonth.ContainsKey(monthYear))
                    ticketsPerMonth[monthYear]++;
                else
                    ticketsPerMonth[monthYear] = 1;
            }

            return ticketsPerMonth;
        }

        private Dictionary<string, decimal> CalculateMoneyPerMonth()
        {
            Dictionary<string, decimal> moneyPerMonth = new Dictionary<string, decimal>();

            foreach (var ticketSale in TicketSales)
            {
                string monthYear = ticketSale.Departure_Date.ToString("MM/yyyy");
                if (moneyPerMonth.ContainsKey(monthYear))
                    moneyPerMonth[monthYear] += ticketSale.Price;
                else
                    moneyPerMonth[monthYear] = ticketSale.Price;
            }

            return moneyPerMonth;
        }

        private int CalculateTotalTickets()
        {
            return TicketSales.Count;
        }

        private decimal CalculateTotalMoney()
        {
            return TicketSales.Sum(ts => ts.Price);
        }

        private void ShowCalculations()
        {
            Dictionary<string, int> ticketsPerTrain = CalculateTicketsPerTrain();
            Dictionary<string, decimal> moneyPerTrain = CalculateMoneyPerTrain();
            Dictionary<string, int> ticketsPerMonth = CalculateTicketsPerMonth();
            Dictionary<string, decimal> moneyPerMonth = CalculateMoneyPerMonth();
            int totalTickets = CalculateTotalTickets();
            decimal totalMoney = CalculateTotalMoney();

            string statistics = "Train tickets:\n";
            foreach (var pair in ticketsPerTrain)
            {
                statistics += $"Train number: {pair.Key}, Tickets sold out: {pair.Value}\n";
            }
            statistics += "\n";

            statistics += "Money for the train:\n";
            foreach (var pair in moneyPerTrain)
            {
                statistics += $"Train number: {pair.Key}, Money earned: {pair.Value:C2}\n";
            }
            statistics += "\n";

            statistics += "Monthly tickets:\n";
            foreach (var pair in ticketsPerMonth)
            {
                statistics += $"Month-year: {pair.Key}, Tickets sold out: {pair.Value}\n";
            }
            statistics += "\n";

            statistics += "Money per month:\n";
            foreach (var pair in moneyPerMonth)
            {
                statistics += $"Month-year: {pair.Key}, Money earned: {pair.Value:C2}\n";
            }
            statistics += "\n";

            statistics += $"Total number of tickets sold: {totalTickets}\n";
            statistics += $"Total amount of money earned: {totalMoney:C2}";

            MessageBox.Show(statistics, "Statistics", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ShowStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowCalculations();
        }
        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Логіка для виходу з програми
            Application.Current.Shutdown();
        }
    }
}
