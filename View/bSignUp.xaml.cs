using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Npgsql;
using BCrypt.Net;

namespace ThriffSignUp.View
{
    public partial class bSignUp : UserControl
    {
        private readonly string connstring = "Host=localhost; Port=5432; Username=postgres; Password=della2908; Database=thriff";

        public bSignUp()
        {
            InitializeComponent();
        }
        private NpgsqlConnection conn;

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            string email = txtEmail.Text;

            // Validasi input kosong
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            // Validasi format email
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Invalid email format.");
                return;
            }

            // Validasi kekuatan password
            if (password.Length < 8 ||
                !password.Any(char.IsDigit) ||
                !password.Any(char.IsUpper) ||
                !password.Any(char.IsLower))
            {
                MessageBox.Show("Password must be at least 8 characters long and contain a mix of uppercase letters, lowercase letters, and numbers.");
                return;
            }

            // Proses pendaftaran
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
            using (var conn = new NpgsqlConnection(connstring))
            {
                try
                {
                    conn.Open();

                    // SQL query untuk insert data
                    string sql = "INSERT INTO Buyer (buyerid, username, password, email) VALUES (@buyerid, @username, @password, @Email)";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        // Generate UUID untuk buyerid
                        Guid buyerId = Guid.NewGuid();

                        // Hash password menggunakan BCrypt
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                        // Tambahkan parameter ke query
                        cmd.Parameters.AddWithValue("buyerid", buyerId);
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("password", hashedPassword);
                        cmd.Parameters.AddWithValue("email", email);

                        // Eksekusi query
                        return cmd.ExecuteNonQuery() == 1;
                    }
                }
                catch (Exception ex)
                {
                    // Tampilkan pesan error
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
