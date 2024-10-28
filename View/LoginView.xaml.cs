using System;
using System.Windows;
using System.Windows.Controls;
using Npgsql;

namespace ThriffSignUp.View
{
    public partial class LoginView : UserControl
    {
        private readonly NpgsqlConnection conn;
        private readonly string connstring = "Host=localhost;Port=5432;Username=postgres;Password=della2908;Database=thriff";

        public LoginView()
        {
            InitializeComponent();
            conn = new NpgsqlConnection(connstring);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Password;

            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("password", password);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    if (result > 0)
                    {
                        MessageBox.Show("Login Successful");
                        // Tambahkan navigasi jika login berhasil
                    }
                    else
                    {
                        MessageBox.Show("Login Failed. Please check your credentials.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
