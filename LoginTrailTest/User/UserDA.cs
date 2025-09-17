using ConnectionToSQL.Helper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginTrailTest.User
{
    public static class UsersDA
    {
        public static Users RetrieveUser(string username)
        {
            MySqlCommand cmd = null;
            DataTable dt = new DataTable();
            MySqlDataAdapter sda = null;

            string query = "SELECT * FROM Users_log WHERE Username = @username LIMIT 1";

            try
            {
                cmd = DBHelper.RunQuery(query, username);

                if (cmd != null)
                {
                    sda = new MySqlDataAdapter(cmd);
                    sda.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        string uName = dr["Username"].ToString();
                        string password = dr["Password"].ToString();
                        string role = dr["Role"].ToString();
                        return new Users(uName, password, role);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обробка помилок, якщо потрібно
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (sda != null) sda.Dispose();
            }
            return null;
        }
    }
}