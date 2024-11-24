using System;
using System.Windows;
using System.Windows.Controls;
using Npgsql;
using BCrypt.Net;

namespace ThriffSignUp.View
{
    public partial class bLogIn : UserControl
    {
        private readonly NpgsqlConnection conn;
        private readonly string connstring = "Host=localhost;Port=5432;Username=postgres;Password=della2908;Database=thriff";

        public bLogIn()
        {
            InitializeComponent();
            conn = new NpgsqlConnection(connstring);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Password;

            // Validasi input kosong
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                conn.Open();
                string query = "SELECT password FROM buyer WHERE username = @username";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("username", username);

                    // Ambil hash password dari database
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        string hashedPassword = result.ToString();

                        // Verifikasi password yang diinput dengan hash dari database
                        if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                        {
                            MessageBox.Show("Login Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Navigasi ke HomeView melalui MainWindow
                            var mainWindow = Application.Current.MainWindow as MainWindow;
                            mainWindow?.NavigateToHomeBuyer(); // Pastikan Anda memiliki metode NavigateToHome() di MainWindow
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
