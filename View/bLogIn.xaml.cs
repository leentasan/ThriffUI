using System;
using System.Windows;
using System.Windows.Controls;
using Npgsql;
using BCrypt.Net;
using System.Configuration;
using ThriffSignUp.Model;

namespace ThriffSignUp.View
{
    public partial class bLogIn : UserControl
    {
        private readonly NpgsqlConnection conn;
        private readonly string connString;
        public bLogIn()
        {
            InitializeComponent();
            connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            conn = new NpgsqlConnection(connString);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                conn.Open();
                string query = "SELECT buyerid, password, address FROM buyer WHERE username = @username";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("username", username);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hashedPassword = reader["password"].ToString();

                            if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                            {
                                int buyerId = Convert.ToInt32(reader["buyerid"]);
                                Session.BuyerId = buyerId;

                                Session.BuyerAddress = reader["address"]?.ToString() ?? string.Empty;
                                Console.WriteLine($"Logged-in BuyerId: {Session.BuyerId}");
                                Console.WriteLine($"Buyer Address: {Session.BuyerAddress}");

                                MessageBox.Show("Login Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                                var mainWindow = Application.Current.MainWindow as MainWindow;
                                mainWindow?.NavigateToHomeBuyer();
                            }
                            else
                            {
                                MessageBox.Show("Login Failed. Incorrect password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Login Failed. Username not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
        }




        private void GoToSignUp(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow)?.NavigateToSignUpPage();
        }

        private void NavigateToHomeBuyer()
        {
            var homeView = new bHome();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = homeView;
            }
        }

    }
}
