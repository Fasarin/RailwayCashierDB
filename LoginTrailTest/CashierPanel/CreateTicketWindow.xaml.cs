using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using ConnectionToSQL.Helper;
using LoginTrailTest.Admin;
using LoginTrailTest.Helper;
using MySql.Data.MySqlClient;

namespace LoginTrailTest.CashierPanel
{
    public partial class CreateTicketWindow : Window
    {
        private string selectedCarType;
        private List<Train> AvailableTrains;
        private List<Schedule> trainSchedules;
        private List<Station> AvailableStations;
        public event EventHandler TicketCreated;

        public CreateTicketWindow()
        {
            InitializeComponent();
            LoadData();
            AvailableStations = DBHelper.GetStationsFromDatabase();
            AvailableTrains = DBHelper.GetTrainsFromDatabase();
            trainSchedules = DBHelper.GetSchedulesFromDatabase();
            // Set initial values for ComboBoxes
            StationComboBox.ItemsSource = AvailableStations;
            StationComboBox.SelectedIndex = 0;
        }

        private void LoadData()
        {
            // Load data when the window is loaded
            StationComboBox.ItemsSource = DBHelper.GetStationsFromDatabase();
            // Add event handlers for date and station selection
            DatePicker.SelectedDateChanged += (sender, args) => UpdateAvailableTrains();
            StationComboBox.SelectionChanged += (sender, args) => UpdateAvailableTrains();
        }

        private void GenerateCode_Click(object sender, RoutedEventArgs e)
        {
            string ticketCode = "UR" + GenerateRandomCode();
            TicketCodeTextBox.Text = ticketCode;
        }

        private string GenerateRandomCode()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void StationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAvailableTrains();
        }

        private void TrainNumberComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TrainNumberComboBox.SelectedItem is Train selectedTrain)
            {
                // Get car types for the selected train
                var carTypes = selectedTrain.Car_Types.Split(',').Select(type => type.Trim()).ToList();
                CarTypeComboBox.ItemsSource = carTypes;
                CarTypeComboBox.IsEnabled = true;
                CarTypeComboBox.SelectedIndex = -1;

                // Populate CarNumberBox with available car numbers
                int numberOfCars = selectedTrain.Number_of_Cars;
                List<int> carNumbers = Enumerable.Range(1, numberOfCars).ToList();
                CarNumberBox.ItemsSource = carNumbers;
                CarNumberBox.IsEnabled = true;
                CarNumberBox.SelectedIndex = -1;
            }
        }

        private void CarNumberBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CarNumberBox.SelectedItem != null && TrainNumberComboBox.SelectedItem != null && CarTypeComboBox.SelectedItem != null && ClockNumberComboBox.SelectedItem != null)
            {
                try
                {
                    int trainId = (int)TrainNumberComboBox.SelectedValue;
                    int carNumber = (int)CarNumberBox.SelectedItem;
                    string carType = CarTypeComboBox.SelectedItem.ToString();
                    DateTime departureDate = DatePicker.SelectedDate.Value;
                    TimeSpan departureTime = TimeSpan.Parse(ClockNumberComboBox.SelectedItem.ToString());

                    // Log parameter values for debugging
                    Console.WriteLine($"trainId: {trainId}, carNumber: {carNumber}, carType: {carType}, departureDate: {departureDate}, departureTime: {departureTime}");

                    List<string> seatCodes = DBHelper.GetAvailableSeatCodes(trainId, carNumber, carType, departureDate, departureTime);
                    if (seatCodes != null && seatCodes.Count > 0)
                    {
                        SeatNumberBox.ItemsSource = seatCodes; // Update SeatNumberBox with filtered seat codes
                        SeatNumberBox.IsEnabled = true;
                        SeatNumberBox.SelectedIndex = -1;
                    }
                    else
                    {
                        MessageBox.Show("No vacancies.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        SeatNumberBox.ItemsSource = null; // Clear SeatNumberBox if no available seats found
                        SeatNumberBox.IsEnabled = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CarTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CarTypeComboBox.SelectedItem != null && TrainNumberComboBox.SelectedItem != null)
            {
                selectedCarType = CarTypeComboBox.SelectedItem.ToString();
                int selectedTrainId = (int)TrainNumberComboBox.SelectedValue;
                // Retrieve the selected train
                var selectedTrain = AvailableTrains.FirstOrDefault(train => train.Train_ID == selectedTrainId);
                if (selectedTrain != null)
                {
                    // Retrieve the route for the selected train
                    var routeForTrain = DBHelper.GetRouteForTrain(selectedTrainId);

                    if (routeForTrain != null)
                    {
                        // Update the price text box
                        switch (selectedCarType)
                        {
                            case "Купе":
                                PriceTextBox.Text = routeForTrain.Price_coupe.ToString();
                                break;
                            case "Плацкарт":
                                PriceTextBox.Text = routeForTrain.Price_plac.ToString();
                                break;
                            default:
                                // Add additional cases for other car types if needed
                                break;
                        }
                        // Activate SeatNumberTextBox
                        SeatNumberBox.IsEnabled = true;
                        // Activate UsernameTextBox
                        UsernameTextBox.IsEnabled = true;
                    }
                }
            }
        }

        private void UpdateAvailableTrains()
        {
            if (StationComboBox.SelectedItem != null && DatePicker.SelectedDate != null)
            {
                // Get selected station and date
                Station selectedStation = (Station)StationComboBox.SelectedItem;
                DateTime selectedDate = DatePicker.SelectedDate.Value;

                // Filter trains based on the selected destination station and date
                var filteredTrainsWithSchedules = AvailableTrains
                    .Where(train => train.Destination_Station == selectedStation.Station_Name)
                    .Select(train => new { Train = train, Schedules = GetSchedulesForTrain(train, selectedDate) })
                    .Where(x => x.Schedules.Any())
                    .Select(x => new { x.Train, x.Schedules })
                    .Distinct()
                    .ToList();

                // Populate TrainNumberComboBox with the filtered trains and their schedules
                TrainNumberComboBox.ItemsSource = filteredTrainsWithSchedules.Select(x => x.Train);
                TrainNumberComboBox.DisplayMemberPath = "Train_Number";
                TrainNumberComboBox.SelectedValuePath = "Train_ID";
                TrainNumberComboBox.IsEnabled = true;
                TrainNumberComboBox.SelectedIndex = -1; // Add this line

                // Set the selected train's schedules as the source for ClockNumberComboBox
                TrainNumberComboBox.SelectionChanged += (sender, args) =>
                {
                    if (TrainNumberComboBox.SelectedItem != null)
                    {
                        int selectedTrainId = (int)TrainNumberComboBox.SelectedValue;
                        var selectedTrainSchedules = filteredTrainsWithSchedules
                            .Where(x => x.Train.Train_ID == selectedTrainId)
                            .SelectMany(x => x.Schedules)
                            .ToList();

                        ClockNumberComboBox.ItemsSource = selectedTrainSchedules.Select(schedule => schedule.Departure_Time.ToString(@"hh\:mm"));
                        ClockNumberComboBox.IsEnabled = true;
                        ClockNumberComboBox.SelectedIndex = -1; // Add this line

                        // Filter platforms based on the selected train and time
                        var filteredPlatforms = selectedTrainSchedules
                            .Select(schedule => schedule.Platform)
                            .Distinct()
                            .ToList();

                        PlatformComboBox.ItemsSource = filteredPlatforms;
                        PlatformComboBox.IsEnabled = true;
                        PlatformComboBox.SelectedIndex = -1; // Add this line
                    }
                };
            }
            else
            {
                TrainNumberComboBox.ItemsSource = null;
                TrainNumberComboBox.IsEnabled = false;
                ClockNumberComboBox.IsEnabled = false;
                PlatformComboBox.IsEnabled = false;
            }
        }

        private List<Schedule> GetSchedulesForTrain(Train train, DateTime departureDate)
        {
            return trainSchedules
                .Where(schedule => schedule.Train_Number == train.Train_Number &&
                                   schedule.Departure_Date.Date == departureDate.Date)
                .ToList();
        }

        private void CreateTicket_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormValid())
            {
                try
                {
                    // Ensure all required fields are filled
                    if (TrainNumberComboBox.SelectedItem == null || CarNumberBox.SelectedItem == null ||
                        CarTypeComboBox.SelectedItem == null || ClockNumberComboBox.SelectedItem == null ||
                        SeatNumberBox.SelectedItem == null || DatePicker.SelectedDate == null)
                    {
                        MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Get values from the form
                    int trainId = (int)TrainNumberComboBox.SelectedValue;
                    int carNumber = (int)CarNumberBox.SelectedItem;
                    string carType = CarTypeComboBox.SelectedItem.ToString();
                    string seatCode = SeatNumberBox.SelectedItem.ToString();
                    string username = UsernameTextBox.Text;
                    DateTime departureDate = DatePicker.SelectedDate.Value;
                    TimeSpan departureTime = TimeSpan.Parse(ClockNumberComboBox.SelectedItem.ToString());
                    decimal price = decimal.Parse(PriceTextBox.Text);

                    // Generate a new ticket code
                    string ticketCode = "UR" + GenerateRandomCode();

                    // Create SQL query for adding a ticket
                    string query = @"
            INSERT INTO Ticket_Sales (Ticket_Code, Route_ID, Platform, Customer_Name, Train_Number, Seat_Type, Seat_Number, Departure_Date, Departure_Time, Price, Car_Number)
            VALUES (@TicketCode, @RouteID, @Platform, @CustomerName, @TrainNumber, @SeatType, @SeatNumber, @DepartureDate, @DepartureTime, @Price, @CarNumber)";

                    // Execute the query with parameters
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.Parameters.AddWithValue("@TicketCode", ticketCode);
                        cmd.Parameters.AddWithValue("@RouteID", trainId); // You can change this if Route_ID is different
                        cmd.Parameters.AddWithValue("@Platform", PlatformComboBox.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@CustomerName", username);
                        cmd.Parameters.AddWithValue("@TrainNumber", TrainNumberComboBox.Text);
                        cmd.Parameters.AddWithValue("@SeatType", carType);
                        cmd.Parameters.AddWithValue("@SeatNumber", seatCode); // No need to convert to number here
                        cmd.Parameters.AddWithValue("@DepartureDate", departureDate);
                        cmd.Parameters.AddWithValue("@DepartureTime", departureTime);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@CarNumber", carNumber); // Add this line

                        DBHelper.connection.Open();
                        cmd.ExecuteNonQuery();
                        DBHelper.connection.Close();
                    }

                    MessageBox.Show("Ticket created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry error code for MySQL
                {
                    MessageBox.Show("Квиток на вибране місце в цей час вже існує.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid username (only letters and spaces are allowed).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            TicketCreated?.Invoke(this, EventArgs.Empty);
        }

        private bool IsFormValid()
        {
            return IsValidUserName(UsernameTextBox.Text);
        }

        private bool IsValidUserName(string username)
        {
            // Valid names should contain only letters and spaces
            return !string.IsNullOrWhiteSpace(username) && Regex.IsMatch(username, @"^[a-zA-Z\u0400-\u04FF\s]+$");
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void AbolitionTicket_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
