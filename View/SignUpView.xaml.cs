using System;
using System.Windows;
using System.Windows.Controls;
using Npgsql;

namespace ThriffSignUp.View
{
    public partial class SignUpView : UserControl
    {
        private readonly string connstring = "Host=localhost; Port=5432; Username=postgres; Password=della2908; Database=thriff";

        public SignUpView()
        {
            InitializeComponent();
        }
        private NpgsqlConnection conn;
        string connstring = "Host=localhost; Port:5432; Username= ; Password: ; Database: ";

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

            bool isRegistered = RegisterUser(username, password, email);

            if (isRegistered)
            {
                MessageBox.Show("Sign Up Successful!");
                (Application.Current.MainWindow as MainWindow)?.NavigateToLoginPage();
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
                    string sql = "INSERT INTO Buyer (buyerid, username, password, email) VALUES (@buyerid, @username, @password, @Email)";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        Random random = new Random();
                        int buyerid = random.Next(100000, 999999); 

                        cmd.Parameters.AddWithValue("buyerid", buyerid);
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("password", password);
                        cmd.Parameters.AddWithValue("email", email);

                        return cmd.ExecuteNonQuery() == 1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }

            }
        }

        private void GoToLogin(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow)?.NavigateToLoginPage();
        }
    }
}
