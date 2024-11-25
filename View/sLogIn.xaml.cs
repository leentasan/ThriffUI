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
using ThriffSignUp.Model;


namespace ThriffSignUp.View
{
    /// <summary>
    /// Interaction logic for sLogIn.xaml
    /// </summary>
    public partial class sLogIn : UserControl
    {
        public sLogIn()
        {
            InitializeComponent();
        }
        private readonly NpgsqlConnection conn;
        private readonly string connstring = "Host=localhost; Port=5432; Username=postgres; Password=jena2019; Database=Thriff";

        private void GoToSignUp(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow)?.NavigateToSellerSignUp();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var conn = new NpgsqlConnection(connstring))

                try
            {
                conn.Open();
                string query = "SELECT sellerid FROM seller WHERE username = @username AND password = @password";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("password", password);

                        var result = cmd.ExecuteScalar();

                        if (result != null) // If a valid SellerId is retrieved
                        {

                            Session.LoggedInSellerId = Convert.ToInt32(result); // Save the SellerId in session
                            Session.SellerUsername = username; // Optionally save the username
                            MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            (Application.Current.MainWindow as MainWindow)?.NavigateToSellerHome();
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
    }
}
