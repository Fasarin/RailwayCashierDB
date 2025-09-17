using ConnectionToSQL.Helper;
using LoginTrailTest.Admin;
using System.Collections.ObjectModel;
using System.Windows;

namespace LoginTrailTest.AdminSchedule
{
    public partial class SearchSchedule : Window
    {
        // Припустимо, що у вас є колекція всіх розкладів
        private ObservableCollection<Schedule> allSchedules;

        public SearchSchedule()
        {
            InitializeComponent();
            // Ініціалізуємо колекцію всіх розкладів
            allSchedules = new ObservableCollection<Schedule>(DBHelper.GetSchedulesFromDatabase());
            // Встановлюємо цю колекцію як джерело даних для ListView
            YourListView.ItemsSource = allSchedules;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо значення для пошуку з TextBox
            string searchTerm = SearchTextBox.Text;

            // Фільтруємо розклади за номером потяга
            ObservableCollection<Schedule> filteredSchedules = new ObservableCollection<Schedule>();

            foreach (Schedule schedule in allSchedules)
            {
                if (schedule.Train_Number.Contains(searchTerm))
                {
                    filteredSchedules.Add(schedule);
                }
            }

            // Оновлюємо ListView з відфільтрованими даними
            YourListView.ItemsSource = filteredSchedules;
        }
    }
}
