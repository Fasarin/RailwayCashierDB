using ConnectionToSQL.Helper;
using LoginTrailTest.Admin;
using LoginTrailTest.Helper;
using System.Collections.Generic; // Add this using directive for List<T>
using System.Windows;

namespace LoginTrailTest.AdminRoute
{
    public partial class CreateSeatWindow : Window
    {
        public CreateSeatWindow()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            // Перевірка, чи всі поля заповнені
            if (string.IsNullOrWhiteSpace(TrainNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(CarNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(CarTypeTextBox.Text) ||
                string.IsNullOrWhiteSpace(SeatCodeTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            /// Перевірка, чи в полі номер вагону введено тільки додатні цифри
            if (!IsPositiveInteger(CarNumberTextBox.Text))
            {
                MessageBox.Show("The car number must be a positive integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Перевірка, чи в полі тип вагону введено "Плацкарт" або "Купе"
            if (CarTypeTextBox.Text != "Плацкарт" && CarTypeTextBox.Text != "Купе")
            {
                MessageBox.Show("The type of car must be 'Плацкарт' або 'Купе'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Перевірка, чи в полі код місця введено тільки додатні цифри
            if (!IsPositiveInteger(SeatCodeTextBox.Text))
            {
                MessageBox.Show("The location code must contain only positive numbers.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Отримання даних з текстових полів
            string trainNumber = TrainNumberTextBox.Text;
            string carType = CarTypeTextBox.Text;

            // Check if the train type is available
            List<Train> availableTrains = DBHelper.GetAvailableTrains(carType);
            if (!availableTrains.Exists(t => t.Train_Number == trainNumber))
            {
                MessageBox.Show("Train with type " + carType + " not available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int carNumber;
            if (!int.TryParse(CarNumberTextBox.Text, out carNumber))
            {
                MessageBox.Show("The car number must contain only numbers..", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string seatCode = SeatCodeTextBox.Text;

            // Створення об'єкта місця
            Seats newSeat = new Seats
            {
                Train_Number = trainNumber,
                Car_Number = carNumber,
                Car_Type = carType,
                Seat_Code = seatCode
            };

            // Додавання нового місця в базу даних
            bool success = SeatsOperationHelper.AddSeat(newSeat);
            if (success)
            {
                MessageBox.Show("Location successfully created.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateSeatsTable(); // Оновлення таблиці
                this.Close();
            }
            else
            {
                MessageBox.Show("Error creating place.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод, який перевіряє, чи в рядку є тільки додатні цифри
        private bool IsPositiveInteger(string text)
        {
            return int.TryParse(text, out int value) && value > 0;
        }

        // Метод, який перевіряє, чи в рядку є тільки цифри
        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }

        // Метод, який перевіряє, чи в рядку є тільки букви
        private bool IsAlpha(string text)
        {
            return text.All(char.IsLetter);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Закриття вікна
            this.Close();
        }

        private void UpdateSeatsTable()
        {
            if (Owner is SeatsWindow seatsWindow)
            {
                seatsWindow.LoadSeats();
            }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            UpdateSeatsTable();
        }
    }
}
