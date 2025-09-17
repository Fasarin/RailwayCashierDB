using System;
using System.Collections.Generic;
using System.Windows;
using LoginTrailTest.Admin;
using LoginTrailTest.Helper;

namespace LoginTrailTest.AdminRoute
{
    public partial class SearchRoute : Window
    {
        public SearchRoute()
        {
            InitializeComponent();
        }

        private void SearchButton(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the search text
                string searchText = SearchTextBox.Text;

                // Perform search for routes in the database
                List<Route> searchResults = RouteOperationsHelper.SearchRoute(searchText);

                // Set the search results as the data source for ListView
                YourListView.ItemsSource = searchResults;

                // Message for successful completion of search
                MessageBox.Show("Пошук виконано.", "Результат пошуку", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Виникла помилка: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
