using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using LoginTrailTest.Admin;
using ConnectionToSQL.Helper;

namespace LoginTrailTest.Helper
{
    internal class ScheduleOperationsHelper
    {
        public static void CreateSchedule(Schedule schedule)
        {
            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    // Перевірка наявності поїзда з введеним номером
                    string trainQuery = "SELECT COUNT(*) FROM Train WHERE Train_Number = @TrainNumber";
                    using (MySqlCommand trainCmd = new MySqlCommand(trainQuery, DBHelper.connection))
                    {
                        trainCmd.Parameters.AddWithValue("@TrainNumber", schedule.Train_Number);
                        DBHelper.connection.Open();
                        int trainCount = Convert.ToInt32(trainCmd.ExecuteScalar());
                        DBHelper.connection.Close();

                        if (trainCount == 0)
                        {
                            MessageBox.Show("The train with the number you entered does not exist..", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return; // Повертаємо, не вставляючи розклад, оскільки поїзд не існує
                        }
                    }
                    // Convert the TimeSpan to a DateTime structure
                    DateTime departureDateTime = new DateTime(schedule.Departure_Date.Year, schedule.Departure_Date.Month, schedule.Departure_Date.Day, schedule.Departure_Time.Hours, schedule.Departure_Time.Minutes, 0);

                    string query = "INSERT INTO Schedule (Train_Number, Platform, Departure_Date, Departure_Time) " +
                           "VALUES (@TrainNumber, @Platform, @DepartureDate, @DepartureTime)";

                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@TrainNumber", schedule.Train_Number);
                        cmd.Parameters.AddWithValue("@Platform", schedule.Platform);
                        cmd.Parameters.AddWithValue("@DepartureDate", departureDateTime); // Use the combined DateTime structure
                        cmd.Parameters.AddWithValue("@DepartureTime", new TimeSpan(departureDateTime.Hour, departureDateTime.Minute, 0)); // Create a new TimeSpan value

                        DBHelper.connection.Open();
                        cmd.ExecuteNonQuery();
                        DBHelper.connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DBHelper.connection?.Close();
            }
        }

        public static void UpdateSchedule(Schedule schedule)
        {
            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    string query = "UPDATE Schedule SET Train_Number = @TrainNumber, Platform = @Platform, " +
                                   "Departure_Date = @DepartureDate, Departure_Time = @DepartureTime " + // Removed the trailing comma
                                   "WHERE Schedule_ID = @ScheduleID";

                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.Parameters.AddWithValue("@TrainNumber", schedule.Train_Number);
                        cmd.Parameters.AddWithValue("@Platform", schedule.Platform);
                        cmd.Parameters.AddWithValue("@DepartureDate", schedule.Departure_Date.ToString("yyyy-MM-dd")); // Ensure date format
                        cmd.Parameters.AddWithValue("@DepartureTime", schedule.Departure_Time.ToString(@"hh\:mm\:ss")); // Ensure time format
                        cmd.Parameters.AddWithValue("@ScheduleID", schedule.Schedule_ID);

                        DBHelper.connection.Open();
                        cmd.ExecuteNonQuery();
                        DBHelper.connection.Close();
                    }
                }
            }
            catch (MySqlException mysqlEx)
            {
                MessageBox.Show("A MySQL error occurred: " + mysqlEx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DBHelper.connection?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DBHelper.connection?.Close();
            }
        }

        public static List<Schedule> SearchSchedule(string searchText)
        {
            List<Schedule> schedules = new List<Schedule>();

            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    string query = "SELECT * FROM Schedule WHERE Schedule_ID LIKE @searchText OR Train_Number LIKE @searchText OR Platform LIKE @searchText OR Departure_Date LIKE @searchText";
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                        DBHelper.connection.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Schedule schedule = new Schedule
                                {
                                    Schedule_ID = Convert.ToInt32(reader["Schedule_ID"]),
                                    Train_Number = reader["Train_Number"].ToString(),
                                    Platform = reader["Platform"].ToString(),
                                    Departure_Date = Convert.ToDateTime(reader["Departure_Date"]),
                                    Departure_Time = TimeSpan.Parse(reader["Departure_Time"].ToString())
                                };
                                schedules.Add(schedule);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                DBHelper.connection?.Close();
            }

            return schedules;
        }

        public static void DeleteSchedule(int scheduleID)
        {
            try
            {
                DBHelper.EstablishConnection();
                if (DBHelper.connection != null)
                {
                    string query = "DELETE FROM Schedule WHERE Schedule_ID = @scheduleID";
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.Parameters.AddWithValue("@scheduleID", scheduleID);
                        DBHelper.connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        DBHelper.connection.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("The schedule has been successfully deleted..", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("The schedule with the specified ID was not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when deleting schedule: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DBHelper.connection?.Close();
            }
        }
    }
}