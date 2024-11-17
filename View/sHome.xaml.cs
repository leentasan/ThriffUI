using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ThriffSignUp.Model;

namespace ThriffSignUp.View
{
    public partial class sHome : UserControl
    {
        private ProductService productService;

        public sHome()
        {
            InitializeComponent();
            productService = new ProductService();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                var products = productService.GetAllProducts();
                lstProducts.ItemsSource = products; // Bind data to ListView
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}");
            }
        }

        private void btnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = (Product)lstProducts.SelectedItem;
            if (selectedProduct != null)
            {
                (Application.Current.MainWindow as MainWindow)?.NavigateToProductsEdit(selectedProduct.ProductId);
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow)?.NavigateToProductsEdit();
        }

        private void ViewAllOrders_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Code to display all orders
            MessageBox.Show("Displaying all orders...");
            (Application.Current.MainWindow as MainWindow)?.NavigateToOrders();
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
