using ConnectionToSQL.Helper;
using LoginTrailTest.Admin;
using LoginTrailTest.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace LoginTrailTest.AdminPanel
{
    public partial class CreateTrainWindow : Window
    {
        public CreateTrainWindow()
        {
            InitializeComponent();
            InitializeTrainWindow();
        }
        private void InitializeTrainWindow()
        {
            DepartureStationTextBox.Text = "Харків-Пасажирський";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Отримуємо дані з полів форми
                string trainNumber = TrainNumberTextBox.Text;
                string departureStation = DepartureStationTextBox.Text;
                string destinationStation = DestinationStationTextBox.Text;
                int numberOfCars = int.Parse(NumberOfCarsTextBox.Text);
                string carTypes = CarTypesTextBox.Text;
                int numberOfCoupe = int.Parse(NumberOfCoupeTextBox.Text);
                int numberOfPlac = int.Parse(NumberOfPlacTextBox.Text);

                // Перевірка, щоб значення було більше 0
                if (numberOfCars <= 0)
                {
                    MessageBox.Show("Кількість вагонів бути більше 0.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Перевірка, чи текст містить слова "Купе" або "Плацкарт"
                if (!carTypes.ToLower().Contains("купе") && !carTypes.ToLower().Contains("плацкарт"))
                {
                    MessageBox.Show("Типи вагонів повинні містити або 'Купе', або 'Плацкарт', або обидва.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Створюємо новий об'єкт Train з отриманими даними
                Train newTrain = new Train
                {
                    Train_Number = trainNumber,
                    Departure_Station = departureStation,
                    Destination_Station = destinationStation,
                    Number_of_Cars = numberOfCars,
                    Car_Types = carTypes,
                    Number_of_Coupe = numberOfCoupe,
                    Number_of_Plac = numberOfPlac
                };

                // Додаємо новий поїзд до бази даних
                TrainOperationsHelper.CreateTrain(newTrain);

                // Повідомлення про успішне створення
                MessageBox.Show("Поїзд був успішно створений.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

                // Закриваємо вікно після завершення
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при створенні поїзда: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CancelButton(object sender, RoutedEventArgs e)
        {
            // Закриваємо поточне вікно без додавання нового маршруту
            this.Close();
        }
        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Дозволяємо вводити лише цифри
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }
        private void LettersOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Перевіряємо, чи є символ літерою
            if (!IsLetter(e.Text))
            {
                e.Handled = true; // Якщо символ не є літерою, відміна введення
            }
        }

        private bool IsLetter(string text)
        {
            // Перевіряємо, чи є текст літерою
            foreach (char c in text)
            {
                if (!char.IsLetter(c))
                {
                    return false; // Якщо хоча б один символ не є літерою, повертаємо false
                }
            }
            return true; // Всі символи є літерами
        }
    }
}
