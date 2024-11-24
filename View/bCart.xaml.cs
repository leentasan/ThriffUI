using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class bCart : UserControl
    {
        // Collection to store items in the cart
        public ObservableCollection<CartItem> CartItems { get; set; }

        public bCart()
        {
            InitializeComponent();
            DataContext = this; // Tambahkan baris ini
            CartItems = new ObservableCollection<CartItem>();

            // Sample data for testing
            CartItems.Add(new CartItem { ProductName = "Product 1", Price = "$10.00", Image = "Images/ILUSTRASI.png" });
            CartItems.Add(new CartItem { ProductName = "Product 2", Price = "$15.00", Image = "Images/ILUSTRASI.png" });

            // Set the data context for binding
            cartItemsList.ItemsSource = CartItems;
            UpdateTotalPrice();
        }


        // Event handler for "Remove" button
        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var item = button?.DataContext as CartItem;
            if (item != null)
            {
                CartItems.Remove(item);
                UpdateTotalPrice();
                MessageBox.Show($"{item.ProductName} removed from cart.");
            }
        }

        // Event handler for "Checkout" button
        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            // Here you could navigate to another view or process the checkout
            MessageBox.Show("Proceeding to checkout.");
        }

        // Method to update the total price display
        private void UpdateTotalPrice()
        {
            double total = 0;
            foreach (var item in CartItems)
            {
                // Assuming Price is stored as a string in format "$XX.XX", you may need to parse it
                if (double.TryParse(item.Price.Replace("$", ""), out double price))
                {
                    total += price;
                }
            }
            totalPrice.Text = $"${total:0.00}";
        }

        private void BackArrow_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Content = new bHome();
            }
        }


    }

    // Model class for cart items
    public class CartItem
    {
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
    }
}