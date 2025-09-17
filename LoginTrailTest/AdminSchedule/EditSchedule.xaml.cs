using LoginTrailTest.Admin;
using LoginTrailTest.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LoginTrailTest.AdminSchedule
{
    public partial class EditSchedule : Window
    {
        private List<string> selectedDaysOfWeek;
        private Schedule schedule;

        public EditSchedule(Schedule selectedSchedule)
        {
            InitializeComponent();
            this.schedule = selectedSchedule;

            // Заповнюємо текстові поля даними поточного розкладу
            TrainNumberTextBox.Text = schedule.Train_Number;
            PlatformTextBox.Text = schedule.Platform;

            // Встановлюємо вибрану дату у DatePicker
            DepartureDatePicker.SelectedDate = schedule.Departure_Date;

            // Ініціалізуємо selectedDaysOfWeek на основі днів із поточного розкладу
            selectedDaysOfWeek = new List<string> { schedule.Departure_Date.DayOfWeek.ToString().Substring(0, 3).ToUpper() };
            
            // Встановлюємо вибрану дату у DatePicker
            DepartureDatePicker.SelectedDate = schedule.Departure_Date;

            // Встановлюємо години та хвилини у відповідні TextBox
            HourTextBox.Text = schedule.Departure_Time.Hours.ToString();
            MinuteTextBox.Text = schedule.Departure_Time.Minutes.ToString();

        }

        private void SaveButtonSchedule(object sender, RoutedEventArgs e)
        {
            try
            {
                // Оновлюємо дані розкладу з форми
                schedule.Train_Number = TrainNumberTextBox.Text;
                schedule.Platform = PlatformTextBox.Text;

                // Отримуємо вибрану дату з DatePicker
                schedule.Departure_Date = DepartureDatePicker.SelectedDate ?? DateTime.MinValue;

                int hours, minutes;
                if (!int.TryParse(HourTextBox.Text, out hours) || !int.TryParse(MinuteTextBox.Text, out minutes))
                {
                    MessageBox.Show("Invalid time format. Please enter valid hours and minutes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
                {
                    MessageBox.Show("Invalid time format. Please enter hours between 0 and 23 and minutes between 0 and 59.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                schedule.Departure_Time = new TimeSpan(hours, minutes, 0);

                // Оновлюємо розклад у базі даних
                ScheduleOperationsHelper.UpdateSchedule(schedule);

                // Оновлюємо дані у вікні ScheduleWindow
                ScheduleWindow scheduleWindow = Application.Current.Windows.OfType<ScheduleWindow>().FirstOrDefault();
                scheduleWindow?.RefreshData();

                // Закриваємо поточне вікно
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButtonSchedule(object sender, RoutedEventArgs e)
        {
            // Закриваємо поточне вікно без оновлення розкладу
            this.Close();
        }
    }
}