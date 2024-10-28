using System;
using System.Windows;
using Npgsql;

namespace WPF_LoginForm.View
{
    public partial class SignUpView : Window
    {
        public SignUpView()
        {
            InitializeComponent();
        }
        private NpgsqlConnection conn;
        string connstring = "Host=localhost; Port:5432; Username=postgres ; Password:della2908 ; Database:thriff ";

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            // Ambil input dari pengguna
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            string email = txtEmail.Text;

            // Validasi sederhana (kamu bisa tambahkan lebih banyak validasi)
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            // Logika untuk menyimpan pengguna baru ke database
            // Contoh sederhana untuk menambahkan data (ini dapat diganti dengan koneksi ke database)
            bool isRegistered = RegisterUser(username, password, email);

            if (isRegistered)
            {
                MessageBox.Show("Sign Up Successful!");
                // Kembali ke halaman Login
                LoginView loginView = new LoginView();
                loginView.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sign Up Failed. Try Again.");
            }
        }

        // Fungsi sederhana untuk menyimpan data pengguna (bisa diubah dengan database)
        private bool RegisterUser(string username, string password, string email)
        {
            try
            {
                // Open database connection
                conn = new NpgsqlConnection(connstring);
                conn.Open();

                // SQL Insert statement with parameters
                string sql = "INSERT INTO Users (username, password, email) VALUES (@username, @password, @Email)";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("password", password);
                    cmd.Parameters.AddWithValue("email", email);

                    // Execute the query and check if insertion was successful
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("User successfully registered!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Insert FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return false;
        }

        private void GoToLogin(object sender, RoutedEventArgs e)
        {
            // Berpindah ke halaman Login
            LoginView loginView = new LoginView();
            loginView.Show();
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

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
