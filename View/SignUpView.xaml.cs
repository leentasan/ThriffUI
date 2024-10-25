using System;
using System.Windows;

namespace WPF_LoginForm.View
{
    public partial class SignUpView : Window
    {
        public SignUpView()
        {
            InitializeComponent();
        }

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
                // Implementasi menyimpan data ke database (contoh dengan file atau list sementara)
                // Database real bisa menggantikan logika ini
                // Misalnya:
                // INSERT INTO Buyer (Username, Password, Email) VALUES(username, password, email);

                return true; // Jika sukses menyimpan ke database
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Menampilkan error jika ada
                return false; // Jika gagal
            }
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
