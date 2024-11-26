using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ThriffSignUp.Model;

namespace ThriffSignUp.View
{
    public partial class bCart : UserControl
    {
        public ObservableCollection<CartItem> CartItems { get; set; }
        public ObservableCollection<GroupedCart> GroupedCartItems { get; set; } = new ObservableCollection<GroupedCart>();

        public bCart()
        {
            InitializeComponent();
            DataContext = this;
            CartItems = new ObservableCollection<CartItem>();
            LoadCartData();
            GroupCartItems();
            groupedCartItems.ItemsSource = GroupedCartItems; // Bind data ke UI
            UpdateTotalPrice();
        }

        private void LoadCartData()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                using (var conn = new Npgsql.NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            c.cartid AS CartId,  -- Perbaiki nama kolom di sini
                            p.name AS ProductName, 
                            p.price AS Price, 
                            p.image_path AS Image, 
                            s.sellerid AS SellerId, 
                            s.username AS SellerName
                        FROM cart c
                        JOIN product p ON c.productid = p.productid
                        JOIN seller s ON p.sellerid = s.sellerid
                        WHERE c.buyerid = @BuyerId";

                    using (var cmd = new Npgsql.NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("BuyerId", Session.BuyerId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CartItems.Add(new CartItem
                                {
                                    CartId = Convert.ToInt32(reader["CartId"]), // Ambil nilai dari cartid
                                    ProductName = reader["ProductName"].ToString(),
                                    Price = $"${reader["Price"]}",
                                    Image = reader["Image"]?.ToString() ?? "/Images/ILUSTRASI.png",
                                    SellerId = Convert.ToInt32(reader["SellerId"]),
                                    SellerName = reader["SellerName"].ToString()
                                });
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cart data: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void GroupCartItems()
        {
            var grouped = CartItems.GroupBy(item => item.SellerId)
                                   .Select(g => new GroupedCart
                                   {
                                       Key = g.First().SellerName,
                                       SellerId = g.Key,
                                       Value = new ObservableCollection<CartItem>(g)
                                   }).ToList();

            GroupedCartItems.Clear();
            foreach (var group in grouped)
            {
                GroupedCartItems.Add(group);
            }
        }

        private void UpdateTotalPrice()
        {
            double total = CartItems.Sum(item => double.TryParse(item.Price.Replace("$", ""), out double price) ? price : 0);
            totalPrice.Text = $"${total:0.00}";
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            var selectedGroup = GroupedCartItems.FirstOrDefault(g => g.IsSelected);
            if (selectedGroup == null)
            {
                MessageBox.Show("Please select a seller to proceed to checkout.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedGroup.Value == null || !selectedGroup.Value.Any())
            {
                MessageBox.Show("Selected seller has no items in the cart.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Session.TotalAmount = selectedGroup.Value.Sum(item =>
            {
                if (double.TryParse(item.Price.Replace("$", ""), out double price))
                {
                    return price;
                }
                else
                {
                    Console.WriteLine($"Invalid price format for product: {item.ProductName}");
                    return 0;
                }
            });

            Console.WriteLine($"Total Amount Before Checkout: Rp{Session.TotalAmount}");

            Session.SellerId = selectedGroup.SellerId;
            Session.ProductId = selectedGroup.Value.First().ProductId;

            Console.WriteLine($"Selected SellerId: {Session.SellerId}");
            Console.WriteLine($"Selected ProductId: {Session.ProductId}");

            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.Content = new bCheckout();
            }
            else
            {
                MessageBox.Show("Unable to navigate to the checkout page.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button?.DataContext as CartItem;

            if (item != null)
            {
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                    using (var conn = new Npgsql.NpgsqlConnection(connectionString))
                    {
                        conn.Open();

                        string deleteQuery = "DELETE FROM cart WHERE cartid = @CartId";
                        using (var cmd = new Npgsql.NpgsqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("CartId", item.CartId);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                CartItems.Remove(item);
                                UpdateTotalPrice();
                                GroupCartItems();
                            }
                            else
                            {
                                MessageBox.Show("Failed to remove item from the database. It may not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error removing item: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }





        private void BackArrow_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {

                mainWindow.Content = new bHome();
            }
            else
            {
                MessageBox.Show("Unable to navigate back. MainWindow not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SellerSelectionChanged(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.Tag != null)
            {
                int selectedSellerId = int.Parse(radioButton.Tag.ToString());

                foreach (var group in GroupedCartItems)
                {
                    group.IsSelected = group.SellerId == selectedSellerId;
                }

                Console.WriteLine($"Selected SellerId: {selectedSellerId}");
            }
        }

    }


    public class GroupedCart
    {
        public string Key { get; set; }
        public int SellerId { get; set; }
        public ObservableCollection<CartItem> Value { get; set; }
        public bool IsSelected { get; set; }
    }

    public class CartItem
    {
        public int CartId { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
        public int SellerId { get; set; }
        public string SellerName { get; set; }
        public int ProductId { get; set; }
    }


}