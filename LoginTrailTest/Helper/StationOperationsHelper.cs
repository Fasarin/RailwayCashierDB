using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using LoginTrailTest.Admin;
using ConnectionToSQL.Helper;

namespace LoginTrailTest.Helper
{
    internal class StationOperationsHelper
    {
        public static void CreateStation(Station station)
        {
            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    string query = "INSERT INTO Station (Station_Name, City) " +
                                   "VALUES (@StationName, @City)";
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@StationName", station.Station_Name);
                        cmd.Parameters.AddWithValue("@City", station.City);

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

        public static void UpdateStation(Station station)
        {
            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    string query = "UPDATE Station SET Station_Name = @StationName, City = @City " +
                        "WHERE Station_ID = @StationID";

                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.Parameters.AddWithValue("@StationName", station.Station_Name);
                        cmd.Parameters.AddWithValue("@City", station.City);
                        cmd.Parameters.AddWithValue("@StationID", station.Station_ID);

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

        public static List<Station> SearchStation(string searchText)
        {
            List<Station> stations = new List<Station>();

            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    string query = "SELECT * FROM Station WHERE Station_ID LIKE @searchText OR Station_Name LIKE @searchText OR City LIKE @searchText";
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                        DBHelper.connection.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Station station = new Station
                                {
                                    Station_ID = Convert.ToInt32(reader["Station_ID"]),
                                    Station_Name = reader["Station_Name"].ToString(),
                                    City = reader["City"].ToString()
                                };
                                stations.Add(station);
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

            return stations;
        }

        public static void DeleteStation(int stationID)
        {
            try
            {
                DBHelper.EstablishConnection();
                if (DBHelper.connection != null)
                {
                    string query = "DELETE FROM Station WHERE Station_ID = @stationID";
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.Parameters.AddWithValue("@stationID", stationID);
                        DBHelper.connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        DBHelper.connection.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("The station has been successfully removed.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("The station with the specified ID was not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при видаленні станції: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DBHelper.connection?.Close();
            }
        }
    }
}
