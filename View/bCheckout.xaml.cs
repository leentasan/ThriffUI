using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Configuration; // Tambahkan untuk membaca App.config
using Npgsql; // Tambahkan untuk PostgreSQL

namespace ThriffSignUp.View
{
    public partial class bCheckout : UserControl
    {
        public bCheckout()
        {
            InitializeComponent();
        }

        // Event handler untuk button 'Place Order'
        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Your order has been placed successfully!");

            // Navigasi kembali ke HomeView
            var homeView = new bHome();
            var parent = (ContentControl)this.Parent;
            if (parent != null)
            {
                parent.Content = homeView;
            }
        }

        // Event handler untuk button 'Cancel'
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Navigasi kembali ke CartView
            var cartView = new bCart();
            var parent = (ContentControl)this.Parent;
            if (parent != null)
            {
                parent.Content = cartView;
            }
        }

        private void ConfirmPurchase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Ambil koneksi dari App.config
                string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    // Insert transaksi ke database
                    string query = "INSERT INTO transaction (transactiondate, totalamount) VALUES (@Date, @Amount)";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("Date", DateTime.Now);
                        cmd.Parameters.AddWithValue("Amount", 100.00); // Contoh jumlah total (ganti sesuai kebutuhan)

                        cmd.ExecuteNonQuery();
                    }
                }

                // Tampilkan pesan sukses
                MessageBox.Show("Purchase confirmed! Thank you for your order.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Navigasi ke HomeView
                var homeView = new bHome();
                var parent = (ContentControl)this.Parent;
                if (parent != null)
                {
                    parent.Content = homeView;
                }
            }
            catch (Exception ex)
            {
                // Tampilkan pesan error
                MessageBox.Show($"Error confirming purchase: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
