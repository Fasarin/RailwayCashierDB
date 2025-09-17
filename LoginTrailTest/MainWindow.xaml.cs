using ConnectionToSQL.Helper;
using LoginTrailTest.User;
using System.Windows;
using System.Windows.Input;

namespace LoginTrailTest
{
    public partial class MainWindow : Window
    {
        private bool IsMaximize = false;

        public MainWindow()
        {
            InitializeComponent();
            DBHelper.EstablishConnection();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1024;
                    this.Height = 720;

                    IsMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    IsMaximize = true;
                }
            }
        }
        private void txtEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
            }
        }
        private void txtPassword_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
            }

            string password = txtPassword.Text;
            if (password.Length < 6)
            {
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            Users aUser = UsersDA.RetrieveUser(email);

            if (aUser != null && aUser.Password.Equals(password))
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("The Email or Password field cannot be empty.");
                    return;
                }

                if (password.Length < 6)
                {
                    MessageBox.Show("The password is too short. Enter at least 6 characters.");
                    return;
                }

                MessageBox.Show("You've successfully logged in!", "Successful login", MessageBoxButton.OK, MessageBoxImage.Information);

                if (aUser.Role == "Admin")
                {
                    TrainWindow trainWindow = new TrainWindow();
                    trainWindow.Show();
                }
                else if (aUser.Role == "Cashier")
                {
                    CashierWindow cashierWindow = new CashierWindow();
                    cashierWindow.Show();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid email or password. Please try again.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Логіка для виходу з програми
            Application.Current.Shutdown();
        }
    }
}