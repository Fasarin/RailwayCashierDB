using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginTrailTest.User
{
    public class Users
    {
        private string username;
        private string password;
        private string role;

        public Users(string username, string password, string role)
        {
            UserName = username;
            Password = password;
            Role = role;
        }
        public String UserName
        {
            get { return username; }
            set { username = value; }
        }
        public String Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Role
        {
            get { return role; }
            set { role = value; }
        }
    }
}