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
            else
            {
                MessageBox.Show("Please select a product to edit.");
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
            var selectedProduct = lstProducts.SelectedItem as Product;
            if (selectedProduct != null)
            {
                // Ask for confirmation before deleting
                var result = MessageBox.Show($"Are you sure you want to delete {selectedProduct.Name}?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Call the delete method
                    productService.DeleteProduct(selectedProduct.ProductId); // Assume you have a DeleteProduct method in ProductService
                    MessageBox.Show("Product deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Refresh the product list after deletion
                    LoadProducts();
                }
            }
        }
        private void ProductGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected product
            var selectedProduct = lstProducts.SelectedItem as Product;
        }


    }
}
