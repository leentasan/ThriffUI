using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThriffSignUp.View
{
    /// <summary>
    /// Interaction logic for sSignUp.xaml
    /// </summary>
    public partial class sSignUp : UserControl
    {
        public sSignUp()
        {
            InitializeComponent();
        }
        private readonly string connstring = "Host=localhost; Port=5432; Username=postgres; Password=jena2019; Database=Thriff";
        private NpgsqlConnection conn;
        private void GoToLogin(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow)?.NavigateToSellerLogIn();
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string username = txtUser.Text;
            string password = txtPass.Password;
            string email = txtEmail.Text;
            string address = txtAddress.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            bool isRegistered = RegisterUser(username, name, password, email, address);

            if (isRegistered)
            {
                MessageBox.Show("Sign Up Successful!");
                (Application.Current.MainWindow as MainWindow)?.NavigateToSellerLogIn();
            }
            else
            {
                MessageBox.Show("Sign Up Failed. Try Again.");
            }

        }
        private bool RegisterUser(string username, string name, string password, string email, string address)
        {
            using (var conn = new NpgsqlConnection(connstring))
            {
                try
                {
                    conn.Open();
                    string sql = "INSERT INTO Seller (username, name, password, email, address) VALUES (@username, @name, @password, @email, @address)";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("name", name);
                        cmd.Parameters.AddWithValue("password", password);
                        cmd.Parameters.AddWithValue("email", email);
                        cmd.Parameters.AddWithValue("address", address);

                        return cmd.ExecuteNonQuery() == 1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }

            }
        }

    }
}
