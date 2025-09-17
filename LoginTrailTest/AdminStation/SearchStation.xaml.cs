using System;
using System.Collections.ObjectModel;
using System.Windows;
using LoginTrailTest.Admin;
using LoginTrailTest.Helper;

namespace LoginTrailTest.AdminStation
{
    public partial class SearchStation : Window
    {
        // Припустимо, що у вас є колекція всіх станцій
        private ObservableCollection<Station> allStations = GetStationsFromDatabase();

        public SearchStation()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо значення для пошуку з TextBox
            string searchTerm = SearchTextBox.Text;

            // Викликаємо метод для пошуку станцій
            List<Station> filteredStations = StationOperationsHelper.SearchStation(searchTerm);

            // Оновлюємо ListView з відфільтрованими даними
            YourListView.ItemsSource = filteredStations;
        }


        // Припустимо, що у вас є функція для отримання всіх станцій з бази даних
        private static ObservableCollection<Station> GetStationsFromDatabase()
        {
            // Реалізація отримання даних з бази даних
            // Повертаємо пусту колекцію для прикладу
            return new ObservableCollection<Station>();
        }
    }
}
