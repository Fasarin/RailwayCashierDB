using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using LoginTrailTest.Admin;
using ConnectionToSQL.Helper;

namespace LoginTrailTest.Helper
{
    internal class SeatsOperationHelper
    {
        public static bool AddSeat(Seats seat)
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
                        trainCmd.Parameters.AddWithValue("@TrainNumber", seat.Train_Number);
                        DBHelper.connection.Open();
                        int trainCount = Convert.ToInt32(trainCmd.ExecuteScalar());
                        DBHelper.connection.Close();

                        if (trainCount == 0)
                        {
                            MessageBox.Show("The train with the number you entered does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false; // Повертаємо false, оскільки поїзд не існує
                        }
                    }

                    // Якщо поїзд існує, додавайте місце
                    string query = "INSERT INTO Seats (Train_Number, Car_Number, Car_Type, Seat_Code) VALUES (@TrainNumber, @CarNumber, @CarType, @SeatCode)";
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@TrainNumber", seat.Train_Number);
                        cmd.Parameters.AddWithValue("@CarNumber", seat.Car_Number);
                        cmd.Parameters.AddWithValue("@CarType", seat.Car_Type);
                        cmd.Parameters.AddWithValue("@SeatCode", seat.Seat_Code);

                        DBHelper.connection.Open();
                        cmd.ExecuteNonQuery();
                        DBHelper.connection.Close();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding seat: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DBHelper.connection?.Close();
            }
            return false;
        }


        public static bool UpdateSeat(Seats seat)
        {
            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    string query = "UPDATE Seats SET Train_Number = @TrainNumber, Car_Number = @CarNumber, Car_Type = @CarType, Seat_Code = @SeatCode WHERE Seats_ID = @SeatID";
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@TrainNumber", seat.Train_Number);
                        cmd.Parameters.AddWithValue("@CarNumber", seat.Car_Number);
                        cmd.Parameters.AddWithValue("@CarType", seat.Car_Type);
                        cmd.Parameters.AddWithValue("@SeatCode", seat.Seat_Code);
                        cmd.Parameters.AddWithValue("@SeatID", seat.Seats_ID);

                        DBHelper.connection.Open();
                        cmd.ExecuteNonQuery();
                        DBHelper.connection.Close();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating seat: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DBHelper.connection?.Close();
            }
            return false;
        }

        public static bool DeleteSeat(int seatID)
        {
            try
            {
                DBHelper.EstablishConnection();

                if (DBHelper.connection != null)
                {
                    string query = "DELETE FROM Seats WHERE Seats_ID = @SeatID";
                    using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@SeatID", seatID);

                        DBHelper.connection.Open();
                        cmd.ExecuteNonQuery();
                        DBHelper.connection.Close();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting seat: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DBHelper.connection?.Close();
            }
            return false;
        }
    }
}
