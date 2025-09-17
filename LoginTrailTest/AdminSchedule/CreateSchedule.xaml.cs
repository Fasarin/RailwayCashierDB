using LoginTrailTest.Admin;
using LoginTrailTest.Helper;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.ObjectModel;

namespace LoginTrailTest.AdminSchedule
{
    public partial class CreateSchedule : Window
    {
        private List<string> selectedDaysOfWeek = new List<string>();
        public ObservableCollection<TimeSpan> AvailableTimes { get; set; }
        public CreateSchedule()
        {
            InitializeComponent();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            string dayOfWeek = checkBox.Content.ToString();

            if (checkBox.IsChecked == true)
            {
                selectedDaysOfWeek.Add(dayOfWeek);
            }
            else
            {
                selectedDaysOfWeek.Remove(dayOfWeek);
            }
        }

        private void CreateButtonSchedule(object sender, RoutedEventArgs e)
        {
            try
            {
                // Отримуємо значення з полів введення
                string trainNumber = TrainNumberTextBox.Text;
                string platform = PlatformTextBox.Text;
                DateTime departureDate = DepartureDatePicker.SelectedDate ?? DateTime.MinValue; // Отримання обраної дати з DatePicker

                // Отримуємо значення з полів годин та хвилин
                int hours = int.Parse(HourTextBox.Text);
                int minutes = int.Parse(MinuteTextBox.Text);

                TimeSpan departureTime = new TimeSpan(hours, minutes, 0);

                // Створюємо новий об'єкт розкладу
                Schedule newSchedule = new Schedule
                {
                    Train_Number = trainNumber,
                    Platform = platform,
                    Departure_Date = departureDate,
                    Departure_Time = departureTime
                };

                // Викликаємо метод CreateSchedule з ScheduleOperationsHelper
                ScheduleOperationsHelper.CreateSchedule(newSchedule);

                // Оновлюємо дані у вікні ScheduleWindow
                ScheduleWindow scheduleWindow = Application.Current.Windows.OfType<ScheduleWindow>().FirstOrDefault();
                scheduleWindow?.RefreshData();

                // Після створення можна закрити вікно
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButtonSchedule(object sender, RoutedEventArgs e)
        {
            // Скасувати і закрити вікно
            this.Close();
        }
    }
}