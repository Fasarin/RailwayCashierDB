using MySql.Data.MySqlClient;
using LoginTrailTest.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ConnectionToSQL.Helper;

namespace LoginTrailTest.Helper
{
    internal class TrainOperationsHelper
    {
        public static void CreateTrain(Train train)
        {
            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    // Перевірка наявності станції відправлення
                    string departureStationQuery = "SELECT COUNT(*) FROM Station WHERE Station_Name = @DepartureStation";
                    using (MySqlCommand departureStationCmd = new MySqlCommand(departureStationQuery, DBHelper.connection))
                    {
                        departureStationCmd.Parameters.AddWithValue("@DepartureStation", train.Departure_Station);
                        DBHelper.connection.Open();
                        int departureStationCount = Convert.ToInt32(departureStationCmd.ExecuteScalar());
                        DBHelper.connection.Close();

                        if (departureStationCount == 0)
                        {
                            MessageBox.Show("The departure station does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return; // Повертаємо, не вставляючи дані про поїзд, оскільки станція відправлення не існує
                        }
                    }

                    // Перевірка наявності станції прибуття
                    string destinationStationQuery = "SELECT COUNT(*) FROM Station WHERE Station_Name = @DestinationStation";
                    using (MySqlCommand destinationStationCmd = new MySqlCommand(destinationStationQuery, DBHelper.connection))
                    {
                        destinationStationCmd.Parameters.AddWithValue("@DestinationStation", train.Destination_Station);
                        DBHelper.connection.Open();
                        int destinationStationCount = Convert.ToInt32(destinationStationCmd.ExecuteScalar());
                        DBHelper.connection.Close();

                        if (destinationStationCount == 0)
                        {
                            MessageBox.Show("Станція прибуття не існує.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return; // Повертаємо, не вставляючи дані про поїзд, оскільки станція прибуття не існує
                        }
                    }

                    string query = "INSERT INTO Train (Train_Number, Departure_Station, Destination_Station, Number_of_Cars, Car_Types, Number_of_Coupe, Number_of_Plac) " +
                                   "VALUES (@TrainNumber, @DepartureStation, @DestinationStation, @NumberOfCars, @CarTypes, @NumberOfCoupe, @NumberOfPlac)";
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@TrainNumber", train.Train_Number);
                        cmd.Parameters.AddWithValue("@DepartureStation", train.Departure_Station);
                        cmd.Parameters.AddWithValue("@DestinationStation", train.Destination_Station);
                        cmd.Parameters.AddWithValue("@NumberOfCars", train.Number_of_Cars);
                        cmd.Parameters.AddWithValue("@CarTypes", train.Car_Types);
                        cmd.Parameters.AddWithValue("@NumberOfCoupe", train.Number_of_Coupe);
                        cmd.Parameters.AddWithValue("@NumberOfPlac", train.Number_of_Plac);

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

        public static void UpdateTrain(Train train)
        {
            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    string query = "UPDATE Train SET Train_Number = @trainNumber, Departure_Station = @departureStation, " +
                        "Destination_Station = @destinationStation, Number_of_Cars = @numberOfCars, Car_Types = @carTypes, " +
                        "Number_of_Coupe = @numberOfCoupe, Number_of_Plac = @numberOfPlac WHERE Train_ID = @trainID";

                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.Parameters.AddWithValue("@trainNumber", train.Train_Number);
                        cmd.Parameters.AddWithValue("@departureStation", train.Departure_Station);
                        cmd.Parameters.AddWithValue("@destinationStation", train.Destination_Station);
                        cmd.Parameters.AddWithValue("@numberOfCars", train.Number_of_Cars);
                        cmd.Parameters.AddWithValue("@carTypes", train.Car_Types);
                        cmd.Parameters.AddWithValue("@numberOfCoupe", train.Number_of_Coupe);
                        cmd.Parameters.AddWithValue("@numberOfPlac", train.Number_of_Plac);
                        cmd.Parameters.AddWithValue("@trainID", train.Train_ID);

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

        public static List<Train> SearchTrain(string searchText)
        {
            List<Train> trains = new List<Train>();

            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    string query = "SELECT * FROM Train WHERE Train_Number LIKE @searchText OR Departure_Station LIKE @searchText OR Destination_Station LIKE @searchText";
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                        DBHelper.connection.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Train train = new Train
                                {
                                    Train_ID = Convert.ToInt32(reader["Train_ID"]),
                                    Train_Number = reader["Train_Number"].ToString(),
                                    Departure_Station = reader["Departure_Station"].ToString(),
                                    Destination_Station = reader["Destination_Station"].ToString(),
                                    Number_of_Cars = Convert.ToInt32(reader["Number_of_Cars"]),
                                    Car_Types = reader["Car_Types"].ToString(),
                                    Number_of_Coupe = Convert.ToInt32(reader["Number_of_Coupe"]),
                                    Number_of_Plac = Convert.ToInt32(reader["Number_of_Plac"])
                                };
                                trains.Add(train);
                            }
                        }
                        DBHelper.connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DBHelper.connection?.Close();
            }

            return trains;
        }

        public static void DeleteTrain(int trainID)
        {
            try
            {
                DBHelper.EstablishConnection();
                if (DBHelper.connection != null)
                {
                    string query = "DELETE FROM Train WHERE Train_ID = @trainID";
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.Parameters.AddWithValue("@trainID", trainID);
                        DBHelper.connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        DBHelper.connection.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("The train was successfully removed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("The train with the specified ID was not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when deleting a train: " + ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DBHelper.connection?.Close();
            }
        }
    }
}
