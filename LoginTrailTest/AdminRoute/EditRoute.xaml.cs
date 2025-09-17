using System;
using System.Linq;
using System.Windows;
using LoginTrailTest.Admin;
using LoginTrailTest.Helper;

namespace LoginTrailTest.AdminRoute
{
    public partial class EditRoute : Window
    {
        private Route route;

        public EditRoute(Route selectedRoute)
        {
            InitializeComponent();
            this.route = selectedRoute;

            // Заповнюємо текстові поля даними поточного маршруту
            TrainIDTextBox.Text = route.Train_Number;
            StationNameTextBox.Text = route.Station_Name;
            RouteLengthTextBox.Text = route.Route_Length.ToString();
            PriceCoupeTextBox.Text = route.Price_coupe.ToString();
            PricePlacTextBox.Text = route.Price_plac.ToString();
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            try
            {
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
                // Оновлюємо дані маршруту з форми
                route.Train_Number = TrainIDTextBox.Text;
                route.Station_Name = StationNameTextBox.Text;
                route.Route_Length = decimal.Parse(RouteLengthTextBox.Text);
                route.Price_coupe = decimal.Parse(PriceCoupeTextBox.Text);
                route.Price_plac = decimal.Parse(PricePlacTextBox.Text);

                // Оновлюємо маршрут у базі даних
                RouteOperationsHelper.UpdateRoute(route);

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
            // Закриваємо поточне вікно без оновлення маршруту
            this.Close();
        }
    }
}
