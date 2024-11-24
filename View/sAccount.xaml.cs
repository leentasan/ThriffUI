using System;
using System.Configuration;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using Npgsql;
using ThriffSignUp.Model;

namespace ThriffSignUp.View
{
    /// <summary>
    /// Interaction logic for sAccount.xaml
    /// </summary>
    public partial class sAccount : UserControl
    {
        public sAccount()
        {
            InitializeComponent();
            LoadAccount();
        }

        private void LoadAccount()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT sellerid, name, password, username, email, address FROM seller WHERE sellerid = @SellerId";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        int sellerid = Session.LoggedInSellerId;
                        command.Parameters.AddWithValue("@SellerId", sellerid);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Map database fields to the Account model.
                                var account = new Account
                                {
                                    sellerid = reader.GetInt32(reader.GetOrdinal("sellerid")),
                                    name = reader.GetString(reader.GetOrdinal("name")),
                                    password = reader.GetString(reader.GetOrdinal("password")),
                                    username = reader.GetString(reader.GetOrdinal("username")),
                                    email = reader.GetString(reader.GetOrdinal("email")),
                                    address = reader.GetString(reader.GetOrdinal("address"))
                                };

                                // Update UI elements with the loaded data.
                                UsernameTextBox.Text = account.username;
                                NameTextBox.Text = account.name;
                                EmailTextBox.Text = account.email;
                                AddressTextBox.Text = account.address;
                                PasswordBox.Password = account.password;
                            }
                            else
                            {
                                MessageBox.Show("Account not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading account: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                UPDATE seller 
                SET 
                    name = @name, 
                    password = @password, 
                    username = @username, 
                    email = @email, 
                    address = @address 
                WHERE sellerid = @id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        // Bind parameters with updated values from the UI
                        command.Parameters.AddWithValue("@id", Session.LoggedInSellerId); // Assuming seller ID is stored in a session
                        command.Parameters.AddWithValue("@name", NameTextBox.Text);
                        command.Parameters.AddWithValue("@password", PasswordBox.Password);
                        command.Parameters.AddWithValue("@username", UsernameTextBox.Text);
                        command.Parameters.AddWithValue("@email", EmailTextBox.Text);
                        command.Parameters.AddWithValue("@address", AddressTextBox.Text);

                        // Execute the update command
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Account updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            (Application.Current.MainWindow as MainWindow)?.NavigateToSellerHome();
                        }
                        else
                        {
                            MessageBox.Show("No account was updated. Please try again.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
