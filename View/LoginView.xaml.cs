using System;
using System.Windows;
using System.Windows.Input;

namespace WPF_LoginForm.View
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Implementasi login di sini, contohnya mengecek username dan password
            string username = txtUser.Text;
            string password = txtPass.Password;

            // Contoh logika sederhana (ganti dengan koneksi ke database)
            if (username == "buyer" && password == "1234")
            {
                MessageBox.Show("Login Successful");
                // Alihkan ke halaman lain jika berhasil
            }
            else
            {
                MessageBox.Show("Login Failed");
            }
        }

        private void GoToSignUp(object sender, RoutedEventArgs e)
        {
            // Berpindah ke halaman SignUp
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
