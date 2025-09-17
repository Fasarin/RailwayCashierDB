using LoginTrailTest.Admin;
using LoginTrailTest.Helper;
using System.Windows;

namespace LoginTrailTest.AdminStation
{
    public partial class EditStation : Window
    {
        private Station station;

        public EditStation(Station selectedStation)
        {
            InitializeComponent();
            this.station = selectedStation;

            // Заповнюємо текстові поля даними поточної станції
            StationNameTextBox.Text = station.Station_Name;
            CityTextBox.Text = station.City;
        }

        private void SaveButtonStation(object sender, RoutedEventArgs e)
        {
            try
            {
                // Оновлюємо дані станції з форми
                station.Station_Name = StationNameTextBox.Text;
                station.City = CityTextBox.Text;

                // Оновлюємо станцію у базі даних
                StationOperationsHelper.UpdateStation(station);

                // Оновлюємо дані у вікні StationWindow
                StationWindow stationWindow = Application.Current.Windows.OfType<StationWindow>().FirstOrDefault();
                stationWindow?.RefreshData();

                // Закриваємо поточне вікно
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Виникла помилка: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButtonStation(object sender, RoutedEventArgs e)
        {
            // Закриваємо поточне вікно без оновлення станції
            this.Close();
        }
    }
}
