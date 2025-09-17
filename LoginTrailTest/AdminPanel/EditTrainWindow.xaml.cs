using ConnectionToSQL.Helper;
using LoginTrailTest.Admin;
using LoginTrailTest.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace LoginTrailTest.AdminPanel
{
    public partial class EditTrainWindow : Window
    {
        private Train SelectedTrain;

        public EditTrainWindow(Train selectedTrain)
        {
            InitializeComponent();

            // Зберегти вибраний поїзд для подальшого використання
            SelectedTrain = selectedTrain;

            // Заповнити поля редактора з даними вибраного поїзда
            TrainNumberTextBox.Text = selectedTrain.Train_Number;
            DepartureStationTextBox.Text = selectedTrain.Departure_Station;
            DestinationStationTextBox.Text = selectedTrain.Destination_Station;
            NumberOfCarsTextBox.Text = selectedTrain.Number_of_Cars.ToString();
            CarTypesTextBox.Text = selectedTrain.Car_Types;
            Number_of_CoupeTextBox.Text = selectedTrain.Number_of_Coupe.ToString();
            Number_of_PlacTextBox.Text = selectedTrain.Number_of_Plac.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримати нові дані з полів форми
            string trainNumber = TrainNumberTextBox.Text;
            string departureStation = DepartureStationTextBox.Text;
            string destinationStation = DestinationStationTextBox.Text;
            string carTypes = CarTypesTextBox.Text;

            if (!IsPositiveInteger(NumberOfCarsTextBox.Text))
            {
                MessageBox.Show("Кількість вагонів повинна бути додатнім числом.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!IsPositiveInteger(Number_of_CoupeTextBox.Text))
            {
                MessageBox.Show("Кількість купе повинна бути додатнім числом.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!IsPositiveInteger(Number_of_PlacTextBox.Text))
            {
                MessageBox.Show("Кількість місць повинна бути додатнім числом.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Отримати нові дані з полів форми
                SelectedTrain.Train_Number = TrainNumberTextBox.Text;
                SelectedTrain.Departure_Station = DepartureStationTextBox.Text;
                SelectedTrain.Destination_Station = DestinationStationTextBox.Text;
                SelectedTrain.Number_of_Cars = int.Parse(NumberOfCarsTextBox.Text);
                SelectedTrain.Car_Types = CarTypesTextBox.Text;
                SelectedTrain.Number_of_Coupe = int.Parse(Number_of_CoupeTextBox.Text);
                SelectedTrain.Number_of_Plac = int.Parse(Number_of_PlacTextBox.Text);

                // Оновити поїзд в базі даних
                TrainOperationsHelper.UpdateTrain(SelectedTrain);

                MessageBox.Show("Поїзд був успішно оновлений.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

                // Оновлюємо дані в ListView (якщо ви хочете оновити дані в іншому вікні, зробіть це в тому вікні)
                //List<Train> trains = DBHelper.GetTrainsFromDatabase();
                //YourData.ItemsSource = trains;

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при оновленні поїзда: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsPositiveInteger(string text)
        {
            return int.TryParse(text, out int value) && value > 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
