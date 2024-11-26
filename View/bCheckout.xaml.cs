using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using Npgsql;
using ThriffSignUp.Model;

namespace ThriffSignUp.View
{
    public partial class bCheckout : UserControl
    {
        private Dictionary<string, string> cityMappings;

        public bCheckout()
        {
            InitializeComponent();
            LoadBuyerAddress();
            LoadSellerAddress();
            LoadShippingMethods();
            UpdateTotalPrice();
        }

        private void LoadBuyerAddress()
        {
            if (string.IsNullOrEmpty(Session.BuyerAddress))
            {
                MessageBox.Show("Your address is missing. Please update it in your profile or during checkout.",
                                "Missing Address", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                addressTextBox.Text = Session.BuyerAddress;
            }
        }

        private void LoadSellerAddress()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT address FROM seller WHERE sellerid = @SellerId";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("SellerId", Session.SellerId);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            sellerAddressBox.Text = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show($"Seller address not found for SellerId: {Session.SellerId}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading seller address: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadShippingMethods()
        {
            try
            {
                string buyerCityId = Ongkir.GetCityIdByName(Session.BuyerAddress);
                string sellerCityId = Ongkir.GetCityIdByName(sellerAddressBox.Text);

                if (Session.Weight <= 0)
                {
                    Session.Weight = 500;
                }

                var options = Ongkir.GetShippingOptions(sellerCityId, buyerCityId, Session.Weight, "jne");

                if (options == null || options.Count == 0)
                {
                    MessageBox.Show("No shipping options available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Console.WriteLine("Shipping Options Debug:");
                foreach (var option in options)
                {
                    Console.WriteLine($"Text: {option.Text}, Value: {option.Value}");
                }

                shippingMethodComboBox.ItemsSource = options;
                shippingMethodComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading shipping methods: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTotalPrice()
        {
            try
            {
                if (Session.TotalAmount <= 0)
                {
                    Console.WriteLine("Session.TotalAmount belum diatur dengan benar.");
                    Session.TotalAmount = 0;
                }

                double total = Session.TotalAmount;

                if (shippingMethodComboBox.SelectedItem != null)
                {
                    var selectedOption = shippingMethodComboBox.SelectedItem as Ongkir.ShippingOption;

                    if (selectedOption != null)
                    {
                        string shippingCostStr = selectedOption.Text.Split('-')[1]
                            .Replace("$", "").Split('(')[0].Trim();

                        Console.WriteLine($"Parsed Shipping Cost String: {shippingCostStr}");

                        if (double.TryParse(shippingCostStr.Replace(",", ""), out double shippingCost))
                        {
                            Console.WriteLine($"Shipping Cost Parsed: ${shippingCost}");
                            total += shippingCost;
                        }
                        else
                        {
                            Console.WriteLine("Gagal parsing biaya pengiriman.");
                        }
                    }
                }

                Console.WriteLine($"Calculated Total Price: ${total}");
                summaryTotalPrice.Text = $"${total:0,0.00}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating total price: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ConfirmPurchase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string orderQuery = "INSERT INTO \"order\" (status, totalamount, sellerid, productid, buyerid) " +
                                 "VALUES (@Status, @TotalAmount, @SellerId, @ProductId, @BuyerId)";

                            using (var cmd = new NpgsqlCommand(orderQuery, conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("Status", "Pending");
                                cmd.Parameters.AddWithValue("TotalAmount", double.Parse(summaryTotalPrice.Text.Replace("Rp", "").Replace(".", "").Trim()));
                                cmd.Parameters.AddWithValue("SellerId", Session.SellerId);
                                cmd.Parameters.AddWithValue("ProductId", Session.ProductId);
                                cmd.Parameters.AddWithValue("BuyerId", Session.BuyerId);

                                cmd.ExecuteNonQuery();
                            }

                            string productUpdateQuery = "UPDATE product SET stock = stock - 1 WHERE productid = @ProductId AND stock > 0";
                            using (var cmd = new NpgsqlCommand(productUpdateQuery, conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("ProductId", Session.ProductId);
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected == 0)
                                {
                                    throw new Exception("Failed to update product stock. Product may be out of stock.");
                                }
                            }

                            transaction.Commit();

                            MessageBox.Show("Order placed successfully! Thank you for your purchase.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            var homeView = new bHome();
                            var parent = (ContentControl)this.Parent;
                            if (parent != null)
                            {
                                parent.Content = homeView;
                            }
                        }
                        catch (Exception innerEx)
                        {
                            transaction.Rollback();
                            throw new Exception($"Transaction failed: {innerEx.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error confirming purchase: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FakeConfirmPurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Order placed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var cartView = new bCart();
            var parent = (ContentControl)this.Parent;
            if (parent != null)
            {
                parent.Content = cartView;
            }
        }
    }
}
