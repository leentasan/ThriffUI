using System;
using System.Windows;
using Npgsql;

namespace ThriffSignUp.View
{
    public partial class LoginView : Window
    {
        private NpgsqlConnection conn;
        private string connstring = "Host=localhost; Port:5432; Username=postgres ; Password:della2908 ; Database:thriff ";

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

                    int result = (int)cmd.ExecuteScalar();
                    if (result > 0)
                    {
                        MessageBox.Show("Login Successful");
                        // Lakukan pengalihan halaman jika login berhasil
                    }
                    else
                    {
                        MessageBox.Show("Login Failed");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void GoToSignUp(object sender, RoutedEventArgs e)
        {
            SignUpView signUpView = new SignUpView();
            signUpView.Show();
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
