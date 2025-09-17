using LoginTrailTest.Admin;
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

namespace LoginTrailTest.AdminStation
{
    public partial class CreateStation : Window
    {
        public CreateStation()
        {
            InitializeComponent();
        }

        private void CreateButtonStation(object sender, RoutedEventArgs e)
        {
            try
            {
                // Отримуємо значення з полів введення
                string stationName = StationNameTextBox.Text;
                string city = CityTextBox.Text;

                // Створюємо новий об'єкт станції
                Station newStation = new Station
                {
                    Station_Name = stationName,
                    City = city
                };

                // Викликаємо метод CreateStation з StationOperationsHelper
                StationOperationsHelper.CreateStation(newStation);

                // Після створення можна закрити вікно
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Виникла помилка: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButtonStation(object sender, RoutedEventArgs e)
        {
            // Скасувати і закрити вікно
            this.Close();
        }
    }
}