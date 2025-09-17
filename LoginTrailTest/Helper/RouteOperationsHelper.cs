using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using LoginTrailTest.Admin;
using ConnectionToSQL.Helper;

namespace LoginTrailTest.Helper
{
    internal class RouteOperationsHelper
    {
        public static void CreateRoute(Route route)
        {
            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    // Перевірка наявності поїзда з введеним номером
                    string trainQuery = "SELECT COUNT(*) FROM Train WHERE Train_Number = @TrainNumber AND Destination_Station = @DestinationStation";
                    using (MySqlCommand trainCmd = new MySqlCommand(trainQuery, DBHelper.connection))
                    {
                        trainCmd.Parameters.AddWithValue("@TrainNumber", route.Train_Number);
                        trainCmd.Parameters.AddWithValue("@DestinationStation", route.Station_Name);
                        DBHelper.connection.Open();
                        int trainCount = Convert.ToInt32(trainCmd.ExecuteScalar());
                        DBHelper.connection.Close();

                        if (trainCount == 0)
                        {
                            MessageBox.Show("Поїзд з введеним номером і кінцевою станцією не існує.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return; // Return if the train does not exist
                        }
                    }

                    // Перевірка наявності станції з введеною назвою
                    string stationQuery = "SELECT COUNT(*) FROM Station WHERE Station_Name = @StationName";
                    using (MySqlCommand stationCmd = new MySqlCommand(stationQuery, DBHelper.connection))
                    {
                        stationCmd.Parameters.AddWithValue("@StationName", route.Station_Name);
                        DBHelper.connection.Open();
                        int stationCount = Convert.ToInt32(stationCmd.ExecuteScalar());
                        DBHelper.connection.Close();

                        if (stationCount == 0)
                        {
                            MessageBox.Show("Станція з введеною назвою не існує.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return; // Повертаємо, не вставляючи маршрут, оскільки станція не існує
                        }
                    }

                    string query = "INSERT INTO Route (Train_ID, Station_ID, Route_Length, Price_coupe, Price_plac) " +
                                   "VALUES ((SELECT Train_ID FROM Train WHERE Train_Number = @TrainNumber), " +
                                   "(SELECT Station_ID FROM Station WHERE Station_Name = @StationName), " +
                                   "@RouteLength, @PriceCoupe, @PricePlac)";
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@TrainNumber", route.Train_Number);
                        cmd.Parameters.AddWithValue("@StationName", route.Station_Name);
                        cmd.Parameters.AddWithValue("@RouteLength", route.Route_Length);
                        cmd.Parameters.AddWithValue("@PriceCoupe", route.Price_coupe);
                        cmd.Parameters.AddWithValue("@PricePlac", route.Price_plac);

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

        public static void UpdateRoute(Route route)
        {
            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    // Ensure the subqueries return a single value by adding LIMIT 1
                    string query = @"UPDATE Route 
                             SET Train_ID = (SELECT Train_ID FROM Train WHERE Train_Number = @TrainNumber LIMIT 1), 
                                 Station_ID = (SELECT Station_ID FROM Station WHERE Station_Name = @StationName LIMIT 1), 
                                 Route_Length = @RouteLength, 
                                 Price_coupe = @PriceCoupe, 
                                 Price_plac = @PricePlac 
                             WHERE Route_ID = @RouteID";

                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.Parameters.AddWithValue("@TrainNumber", route.Train_Number);
                        cmd.Parameters.AddWithValue("@StationName", route.Station_Name);
                        cmd.Parameters.AddWithValue("@RouteLength", route.Route_Length);
                        cmd.Parameters.AddWithValue("@PriceCoupe", route.Price_coupe);
                        cmd.Parameters.AddWithValue("@PricePlac", route.Price_plac);
                        cmd.Parameters.AddWithValue("@RouteID", route.Route_ID);

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


        public static List<Route> SearchRoute(string searchText)
        {
            List<Route> routes = new List<Route>();

            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    string query = "SELECT r.*, t.Train_Number, s.Station_Name " +
                                   "FROM Route r " +
                                   "INNER JOIN Train t ON r.Train_ID = t.Train_ID " +
                                   "INNER JOIN Station s ON r.Station_ID = s.Station_ID " +
                                   "WHERE t.Train_Number LIKE @searchText OR " +
                                   "s.Station_Name LIKE @searchText";

                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                        DBHelper.connection.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Route route = new Route
                                {
                                    Route_ID = Convert.ToInt32(reader["Route_ID"]),
                                    Train_Number = Convert.ToString(reader["Train_Number"]),
                                    Station_Name = Convert.ToString(reader["Station_Name"]),
                                    Route_Length = Convert.ToDecimal(reader["Route_Length"]),
                                    Price_coupe = Convert.ToDecimal(reader["Price_coupe"]),
                                    Price_plac = Convert.ToDecimal(reader["Price_plac"])
                                };
                                routes.Add(route);
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

            return routes;
        }

        public static void DeleteRoute(int routeID)
        {
            try
            {
                DBHelper.EstablishConnection();
                if (DBHelper.connection != null)
                {
                    string query = "DELETE FROM Route WHERE Route_ID = @routeID";
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.Parameters.AddWithValue("@routeID", routeID);
                        DBHelper.connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        DBHelper.connection.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Маршрут був успішно видалений.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Маршрут з вказаним ID не був знайдений.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при видаленні маршруту: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                DBHelper.connection?.Close();
            }
        }
    }
}
