using LoginTrailTest.Admin;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace ConnectionToSQL.Helper
{
    public static class DBHelper
    {
        public static MySqlConnection connection;
        private static MySqlCommand cmd = null;
        private static DataTable dt;
        private static MySqlDataAdapter sda;
        public static bool EstablishConnection()
        {
            try
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = "127.0.0.1";
                builder.Port = 3308;
                builder.UserID = "root";
                builder.Password = "";
                builder.Database = "railway";

                connection = new MySqlConnection(builder.ToString());
                connection.Open();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failed");
                return false;
            }
        }

        public static MySqlCommand RunQuery(string query, string username)
        {
            try
            {
                if (connection != null)
                {
                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            return cmd;
        }
        public static MySqlCommand RunQuery(string query)
        {
            try
            {
                if (connection != null)
                {
                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            return cmd;
        }
        public static List<Train> GetTrainsFromDatabase()
        {
            List<Train> trains = new List<Train>();

            try
            {
                EstablishConnection();

                if (connection != null)
                {
                    string query = "SELECT * FROM Train";
                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            return trains;
        }
        public static List<Route> GetRoutesFromDatabase()
        {
            List<Route> routes = new List<Route>();

            try
            {
                EstablishConnection();

                if (connection != null)
                {
                    string query = @"
                SELECT r.Route_ID, r.Route_Length, r.Price_coupe, r.Price_plac,
                       t.Train_Number, s.Station_Name
                FROM Route r
                INNER JOIN Train t ON r.Train_ID = t.Train_ID
                INNER JOIN Station s ON r.Station_ID = s.Station_ID";

                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Route route = new Route
                            {
                                Route_ID = Convert.ToInt32(reader["Route_ID"]),
                                Train_Number = reader["Train_Number"] != DBNull.Value ? reader["Train_Number"].ToString() : string.Empty,
                                Station_Name = reader["Station_Name"] != DBNull.Value ? reader["Station_Name"].ToString() : string.Empty,
                                Route_Length = reader["Route_Length"] != DBNull.Value ? Convert.ToDecimal(reader["Route_Length"]) : 0,
                                Price_coupe = reader["Price_coupe"] != DBNull.Value ? Convert.ToDecimal(reader["Price_coupe"]) : 0,
                                Price_plac = reader["Price_plac"] != DBNull.Value ? Convert.ToDecimal(reader["Price_plac"]) : 0
                            };
                            routes.Add(route);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            return routes;
        }
        public static List<Schedule> GetSchedulesFromDatabase()
        {
            List<Schedule> schedules = new List<Schedule>();

            try
            {
                EstablishConnection();

                if (connection != null)
                {
                    string query = "SELECT * FROM Schedule";
                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while retrieving schedules: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return schedules;
        }
        public static List<Station> GetStationsFromDatabase()
        {
            List<Station> stations = new List<Station>();

            try
            {
                EstablishConnection();

                if (connection != null)
                {
                    string query = "SELECT * FROM Station";
                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            return stations;
        }
        public static List<TicketSale> GetTicketSalesFromDatabase()
        {
            List<TicketSale> ticketSales = new List<TicketSale>();

            try
            {
                EstablishConnection();

                if (connection != null)
                {
                    string query = @"
            SELECT ts.Sale_ID, ts.Ticket_Code, r.Route_ID, s.Station_Name, ts.Platform, 
                   ts.Customer_Name, ts.Train_Number, ts.Seat_Type, ts.Seat_Number, 
                   ts.Departure_Date, ts.Departure_Time, ts.Price, ts.Car_Number
            FROM Ticket_Sales ts
            INNER JOIN Route r ON ts.Route_ID = r.Route_ID
            INNER JOIN Station s ON r.Station_ID = s.Station_ID";

                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TicketSale ticketSale = new TicketSale
                            {
                                Sale_ID = Convert.ToInt32(reader["Sale_ID"]),
                                Ticket_Code = reader["Ticket_Code"].ToString(),
                                Route_ID = Convert.ToInt16(reader["Route_ID"]),
                                Station_Name = reader["Station_Name"].ToString(), // Use Station_Name instead of Route_ID
                                Platform = reader["Platform"].ToString(),
                                Customer_Name = reader["Customer_Name"].ToString(),
                                Train_Number = reader["Train_Number"].ToString(),
                                Car_Number = Convert.ToInt32(reader["Car_Number"]), // Додано
                                Seat_Type = reader["Seat_Type"].ToString(),
                                Seat_Number = Convert.ToInt32(reader["Seat_Number"]),
                                Departure_Date = Convert.ToDateTime(reader["Departure_Date"]),
                                Departure_Time = TimeSpan.Parse(reader["Departure_Time"].ToString()),
                                Price = Convert.ToDecimal(reader["Price"])
                            };
                            ticketSales.Add(ticketSale);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            return ticketSales;
        }
        public static List<Seats> GetSeatsFromDatabase()
        {
            List<Seats> seats = new List<Seats>();

            try
            {
                EstablishConnection();

                if (connection != null)
                {
                    string query = "SELECT * FROM Seats";
                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Seats seat = new Seats
                            {
                                Seats_ID = Convert.ToInt32(reader["Seats_ID"]),
                                Train_Number = reader["Train_Number"].ToString(),
                                Car_Number = Convert.ToInt32(reader["Car_Number"]),
                                Car_Type = reader["Car_Type"].ToString(),
                                Seat_Code = reader["Seat_Code"].ToString()
                            };
                            seats.Add(seat);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while retrieving seats: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            return seats;
        }

        public static List<Train> GetAvailableTrains(DateTime date, string destinationStation)
        {
            List<Train> availableTrains = new List<Train>();

            try
            {
                EstablishConnection();

                if (connection != null)
                {
                    string query = @"
                SELECT *
                FROM Train
                WHERE Destination_Station = @DestinationStation
                AND Train_ID IN (
                    SELECT Train_ID
                    FROM Schedule
                    WHERE Departure_Date = @DepartureDate
                )";

                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@DestinationStation", destinationStation);
                    cmd.Parameters.AddWithValue("@DepartureDate", date);

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
                            availableTrains.Add(train);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while retrieving available trains: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            return availableTrains;
        }

        public static List<Schedule> GetSchedulesForTrain(int trainID, DateTime departureDate)
        {
            List<Schedule> schedules = new List<Schedule>();

            try
            {
                EstablishConnection();

                if (connection != null)
                {
                    string query = @"
                SELECT *
                FROM Schedule
                WHERE Train_Number = @TrainNumber
                AND Departure_Date = @DepartureDate";

                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@TrainNumber", trainID);
                    cmd.Parameters.AddWithValue("@DepartureDate", departureDate);

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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while retrieving schedules: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }

            return schedules;
        }
        public static Route GetRouteForTrain(int trainId)
        {
            Route route = null;

            try
            {
                EstablishConnection();

                if (connection != null)
                {
                    string query = @"
         SELECT r.Route_ID, r.Train_ID, t.Train_Number, s.Station_Name, r.Route_Length, r.Price_coupe, r.Price_plac
         FROM Route r
         INNER JOIN Train t ON r.Train_ID = t.Train_ID
         INNER JOIN Station s ON r.Station_ID = s.Station_ID
         WHERE r.Train_ID = @TrainID";

                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@TrainID", trainId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            route = new Route
                            {
                                Route_ID = Convert.ToInt32(reader["Route_ID"]),
                                Train_ID = Convert.ToInt32(reader["Train_ID"]),
                                Train_Number = reader["Train_Number"].ToString(),
                                Station_Name = reader["Station_Name"].ToString(),
                                Route_Length = Convert.ToDecimal(reader["Route_Length"]),
                                Price_coupe = Convert.ToDecimal(reader["Price_coupe"]),
                                Price_plac = Convert.ToDecimal(reader["Price_plac"])
                            };
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }

            return route;
        }
        public static List<string> GetAvailableSeatCodes(int trainId, int carNumber, string carType, DateTime departureDate, TimeSpan departureTime)
        {
            List<string> seatCodes = new List<string>();

            try
            {
                EstablishConnection();

                if (connection != null)
                {
                    string query = @"
                SELECT ms.Seat_Code 
                FROM Seats ms
                JOIN Schedule s ON ms.Train_Number = s.Train_Number
                WHERE ms.Train_Number = (SELECT Train_Number FROM Train WHERE Train_ID = @TrainId)
                AND ms.Car_Number = @CarNumber
                AND s.Departure_Date = @DepartureDate
                AND s.Departure_Time = @DepartureTime
                AND ms.Car_Type = @CarType  -- Додано умову для типу вагона
                AND ms.Seat_Code NOT IN (
                    SELECT ts.Seat_Number 
                    FROM Ticket_Sales ts 
                    WHERE ts.Departure_Date = @DepartureDate
                    AND ts.Departure_Time = @DepartureTime
                    AND ts.Train_Number = (SELECT Train_Number FROM Train WHERE Train_ID = @TrainId)
                    AND ts.Car_Number = @CarNumber
                )";

                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@TrainId", trainId);
                    cmd.Parameters.AddWithValue("@CarNumber", carNumber);
                    cmd.Parameters.AddWithValue("@CarType", carType);
                    cmd.Parameters.AddWithValue("@DepartureDate", departureDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@DepartureTime", departureTime.ToString(@"hh\:mm\:ss"));

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            seatCodes.Add(reader["Seat_Code"].ToString());
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return seatCodes;
        }

        public static bool CreateTicketSale(TicketSale ticket)
        {
            try
            {
                EstablishConnection();

                if (connection != null)
                {
                    string query = @"INSERT INTO Ticket_Sales 
                        (Ticket_Code, Route_ID, Train_Number, Seat_Type, Seat_Number, Car_Number, Departure_Date, Departure_Time, Price, Customer_Name)
                        VALUES 
                        (@Ticket_Code, @Route_ID, @Train_Number, @Seat_Type, @Seat_Number, @Car_Number, @Departure_Date, @Departure_Time, @Price, @Customer_Name)";

                    connection.Open();
                    using (cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Ticket_Code", ticket.Ticket_Code);
                        cmd.Parameters.AddWithValue("@Route_ID", ticket.Route_ID);
                        cmd.Parameters.AddWithValue("@Train_Number", ticket.Train_Number);
                        cmd.Parameters.AddWithValue("@Seat_Type", ticket.Seat_Type);
                        cmd.Parameters.AddWithValue("@Seat_Number", ticket.Seat_Number);
                        cmd.Parameters.AddWithValue("@Car_Number", ticket.Car_Number);
                        cmd.Parameters.AddWithValue("@Departure_Date", ticket.Departure_Date);
                        cmd.Parameters.AddWithValue("@Departure_Time", ticket.Departure_Time);
                        cmd.Parameters.AddWithValue("@Price", ticket.Price);
                        cmd.Parameters.AddWithValue("@Customer_Name", ticket.Customer_Name);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        connection.Close();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while creating the ticket: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return false;
        }
        public static bool DeleteTicketSale(int saleId)
        {
            try
            {
                EstablishConnection();

                if (connection != null)
                {
                    string query = "SELECT Departure_Date FROM Ticket_Sales WHERE Sale_ID = @Sale_ID";
                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@Sale_ID", saleId);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        DateTime departureDate = reader.GetDateTime(0);
                        if ((departureDate - DateTime.Now).TotalDays > 1)
                        {
                            reader.Close();

                            query = "DELETE FROM Ticket_Sales WHERE Sale_ID = @Sale_ID";
                            cmd.CommandText = query;
                            cmd.ExecuteNonQuery();

                            connection.Close();
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("You cannot delete a ticket if there is only 1 day left before the departure date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            return false;
        }
        public static List<Train> GetAvailableTrains(string trainType)
        {
            List<Train> availableTrains = new List<Train>();

            try
            {
                EstablishConnection();

                if (connection != null)
                {
                    string query = "SELECT * FROM Train WHERE FIND_IN_SET(@CarType, Car_Types) > 0";
                    connection.Open();
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@CarType", trainType);

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
                            availableTrains.Add(train);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }

            return availableTrains;
        }
    }
}