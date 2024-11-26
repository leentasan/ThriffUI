using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ThriffSignUp.Model;
using System.Configuration;

namespace ThriffSignUp.View
{
    public partial class DetailProduct : UserControl
    {

        public Product Product { get; set; }

        public DetailProduct()
        {
            InitializeComponent();
            DataContext = this;
        }


        public static readonly DependencyProperty ProductIdProperty =
            DependencyProperty.Register("ProductId", typeof(int), typeof(DetailProduct),
                new PropertyMetadata(0, OnProductIdChanged));

        public int ProductId
        {
            get { return (int)GetValue(ProductIdProperty); }
            set { SetValue(ProductIdProperty, value); }
        }

        private static void OnProductIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var detailView = (DetailProduct)d;
            detailView.LoadProductDetails();
        }

        private void LoadProductDetails()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                using (var conn = new Npgsql.NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT name, price, description, image_path FROM product WHERE productid = @ProductId";

                    using (var cmd = new Npgsql.NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("ProductId", ProductId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DataContext = new
                                {
                                    ProductName = reader["name"].ToString(),
                                    ProductPrice = Convert.ToDecimal(reader["price"]),
                                    ProductDescription = reader["description"].ToString(),
                                    ProductImagePath = reader["image_path"]?.ToString() ?? "/Images/ILUSTRASI.png"
                                };
                            }
                            else
                            {
                                MessageBox.Show("Product not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product details: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                using (var conn = new Npgsql.NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    int buyerId = Session.BuyerId;
                    if (buyerId <= 0)
                    {
                        MessageBox.Show("Invalid BuyerId. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    string validateBuyerQuery = "SELECT COUNT(*) FROM buyer WHERE buyerid = @BuyerId";
                    using (var validateCmd = new Npgsql.NpgsqlCommand(validateBuyerQuery, conn))
                    {
                        validateCmd.Parameters.AddWithValue("BuyerId", buyerId);
                        var buyerExists = (long)validateCmd.ExecuteScalar();

                        if (buyerExists == 0)
                        {
                            MessageBox.Show("Invalid BuyerId. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    string checkQuery = "SELECT 1 FROM cart WHERE productid = @ProductId AND buyerid = @BuyerId";
                    using (var cmd = new Npgsql.NpgsqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("ProductId", ProductId);
                        cmd.Parameters.AddWithValue("BuyerId", buyerId);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            MessageBox.Show("Product is already in the cart!");
                        }
                        else
                        {
                            string insertQuery = "INSERT INTO cart (productid, buyerid) VALUES (@ProductId, @BuyerId)";
                            using (var insertCmd = new Npgsql.NpgsqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("ProductId", ProductId);
                                insertCmd.Parameters.AddWithValue("BuyerId", buyerId);
                                insertCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Product added to cart!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product to cart: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.Content = new bCart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to Cart View: {ex.Message}", "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}