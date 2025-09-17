using LoginTrailTest.Admin;
using LoginTrailTest.Helper;
using System;
using System.Windows;

namespace LoginTrailTest.AdminRoute
{
    public partial class EditSeatWindow : Window
    {
        private Seats seatToEdit;

        public EditSeatWindow(Seats seat)
        {
            InitializeComponent();
            seatToEdit = seat;

            // Заповнення полів даними місця для редагування
            TrainNumberTextBox.Text = seat.Train_Number;
            CarNumberTextBox.Text = seat.Car_Number.ToString();
            CarTypeTextBox.Text = seat.Car_Type;
            SeatCodeTextBox.Text = seat.Seat_Code;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримання даних з текстових полів
            string trainNumber = TrainNumberTextBox.Text;
            int carNumber;

            // Перевірка, чи в полі номер вагону введено тільки додатні цифри
            if (!IsPositiveInteger(CarNumberTextBox.Text))
            {
                MessageBox.Show("Номер вагону повинен бути додатнім цілим числом.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                carNumber = int.Parse(CarNumberTextBox.Text); // Конвертуємо строку в ціле число, оскільки вона вже пройшла перевірку на ціле число
            }
            string carType = CarTypeTextBox.Text;
            string seatCode = SeatCodeTextBox.Text;

            // Перевірка, чи всі поля заповнені
            if (string.IsNullOrEmpty(trainNumber) || string.IsNullOrEmpty(carType) || string.IsNullOrEmpty(seatCode))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Перевірка, щоб у CarTypeTextBox були тільки "Плацкарт" або "Купе"
            if (carType != "Плацкарт" && carType != "Купе")
            {
                MessageBox.Show("Тип вагону повинен бути 'Плацкарт' або 'Купе'.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Перевірка, щоб у SeatCodeTextBox були тільки додатні цифри
            if (!IsPositiveInteger(seatCode))
            {
                MessageBox.Show("Код місця повинен містити тільки додатні цифри.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Оновлення об'єкта місця з новими даними
            seatToEdit.Train_Number = trainNumber;
            seatToEdit.Car_Number = carNumber;
            seatToEdit.Car_Type = carType;
            seatToEdit.Seat_Code = seatCode;

            // Оновлення місця в базі даних
            bool success = SeatsOperationHelper.UpdateSeat(seatToEdit);
            if (success)
            {
                MessageBox.Show("Місце успішно оновлено.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateSeatsTable(); // Оновлення таблиці
                this.Close();
            }
            else
            {
                MessageBox.Show("Помилка при оновленні місця.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool IsAllDigits(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        // Метод, який перевіряє, чи в рядку є тільки додатні цифри
        private bool IsPositiveInteger(string text)
        {
            return int.TryParse(text, out int value) && value > 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Закриття вікна
            this.Close();
        }

        private bool IsAllLetters(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                    return false;
            }
            return true;
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
