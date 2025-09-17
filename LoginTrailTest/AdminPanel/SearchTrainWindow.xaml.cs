using ConnectionToSQL.Helper;
using LoginTrailTest.Admin;
using LoginTrailTest.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace LoginTrailTest.AdminPanel
{
    public partial class SearchTrainWindow : Window
    {
        public event EventHandler SearchCompleted;

        public SearchTrainWindow()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо дані для пошуку з поля форми
            string searchKeyword = SearchTextBox.Text;

            // Викликаємо метод для пошуку по базі даних
            List<Train> searchResult = TrainOperationsHelper.SearchTrain(searchKeyword);

            // Встановлюємо результати пошуку як джерело даних для ListView
            SearchResultsListView.ItemsSource = searchResult;
            SearchCompleted?.Invoke(this, EventArgs.Empty);

            // Повідомлення про успішне завершення пошуку
            MessageBox.Show("Пошук завершено.", "Результат пошуку", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Закриваємо вікно при натисканні кнопки "Скасувати"
            this.Close();
        }
    }
}
