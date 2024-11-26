using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Npgsql;
using BCrypt.Net;
using System.Configuration;

namespace ThriffSignUp.View
{
    public partial class bSignUp : UserControl
    {
        private readonly string connString;

        public bSignUp()
        {
            InitializeComponent();
            connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
        }
        private NpgsqlConnection conn;

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            string email = txtEmail.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Invalid email format.");
                return;
            }

            if (password.Length < 8 ||
                !password.Any(char.IsDigit) ||
                !password.Any(char.IsUpper) ||
                !password.Any(char.IsLower))
            {
                MessageBox.Show("Password must be at least 8 characters long and contain a mix of uppercase letters, lowercase letters, and numbers.");
                return;
            }

            bool isRegistered = RegisterUser(username, password, email);

            if (isRegistered)
            {
                MessageBox.Show("Sign Up Successful!");
                (Application.Current.MainWindow as MainWindow)?.NavigateToBuyerLogin();
            }
            else
            {
                MessageBox.Show("Sign Up Failed. Try Again.");
            }
        }

        private bool RegisterUser(string username, string password, string email)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    string sql = "INSERT INTO Buyer (buyerid, username, password, email) VALUES (@buyerid, @username, @password, @Email)";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        Guid buyerId = Guid.NewGuid();

                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                        cmd.Parameters.AddWithValue("buyerid", buyerId);
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("password", hashedPassword);
                        cmd.Parameters.AddWithValue("email", email);

                        return cmd.ExecuteNonQuery() == 1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database Error: {ex.Message}");
                    return false;
                }
            }
        }

        private void GoToLogin(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow)?.NavigateToBuyerLogin();
        }
    }
}
