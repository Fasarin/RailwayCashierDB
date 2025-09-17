using System;
using System.Windows;
using LoginTrailTest.Admin;
using LoginTrailTest.Helper;

namespace LoginTrailTest.AdminRoute
{
    public partial class CreateRoute : Window
    {
        public CreateRoute()
        {
            InitializeComponent();
        }

        private void CreateButton(object sender, RoutedEventArgs e)
        {
            try
            {
                // Збираємо дані з текстових полів
                string trainNumber = TrainIDTextBox.Text;
                string stationName = StationNameTextBox.Text;
                // Перевірка введених даних
                if (!decimal.TryParse(RouteLengthTextBox.Text, out decimal routeLength) || routeLength < 0)
                {
                    MessageBox.Show("Довжина маршруту повинна бути невід'ємним числом.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!decimal.TryParse(PriceCoupeTextBox.Text, out decimal priceCoupe) || priceCoupe < 0)
                {
                    MessageBox.Show("Ціна купе повинна бути невід'ємним числом.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!decimal.TryParse(PricePlacTextBox.Text, out decimal pricePlac) || pricePlac < 0)
                {
                    MessageBox.Show("Ціна плацкарт повинна бути невід'ємним числом.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Створюємо новий маршрут
                Route newRoute = new Route
                {
                    Train_Number = trainNumber,
                    Station_Name = stationName,
                    Route_Length = routeLength,
                    Price_coupe = priceCoupe,
                    Price_plac = pricePlac
                };

                // Додаємо новий маршрут в базу даних
                RouteOperationsHelper.CreateRoute(newRoute);

                // Оновлюємо дані у вікні RouteWindow
                RouteWindow routeWindow = Application.Current.Windows.OfType<RouteWindow>().FirstOrDefault();
                routeWindow?.RefreshData();

                // Закриваємо поточне вікно
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Виникла помилка: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            // Закриваємо поточне вікно без додавання нового маршруту
            this.Close();
        }
    }
}
