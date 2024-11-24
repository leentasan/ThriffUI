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
            DataContext = this; // Set DataContext untuk binding
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

                    // Cek apakah produk sudah ada di cart
                    string checkQuery = "SELECT quantity FROM cart WHERE productid = @ProductId";
                    using (var cmd = new Npgsql.NpgsqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("ProductId", ProductId);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            // Jika sudah ada, update quantity
                            int currentQuantity = Convert.ToInt32(result);
                            string updateQuery = "UPDATE cart SET quantity = @Quantity WHERE productid = @ProductId";
                            using (var updateCmd = new Npgsql.NpgsqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("Quantity", currentQuantity + 1);
                                updateCmd.Parameters.AddWithValue("ProductId", ProductId);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Jika belum ada, tambahkan produk ke cart
                            string insertQuery = "INSERT INTO cart (productid, quantity) VALUES (@ProductId, @Quantity)";
                            using (var insertCmd = new Npgsql.NpgsqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("ProductId", ProductId);
                                insertCmd.Parameters.AddWithValue("Quantity", 1);
                                insertCmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Product added to cart!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product to cart: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}