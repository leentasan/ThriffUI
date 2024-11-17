using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Configuration;
using ThriffSignUp.Model;

namespace ThriffSignUp.View
{
    /// <summary>
    /// Interaction logic for sProduct.xaml
    /// </summary>
    public partial class sProduct : UserControl
    {
        private readonly string connectionString;
        private bool isEditMode = false;
        private int currentProductId = 0;
        private ProductService productService;
        public sProduct()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            productService = new ProductService();
        }
 
        private void btnUpload(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "Image Files|*.bmp;*.jpg;*.png";
                openDialog.FilterIndex = 1;
                if (openDialog.ShowDialog() == true)
                {
                    txtImagePath.Text = openDialog.FileName;
                    productImage.Source = new BitmapImage(new Uri(openDialog.FileName));

                    string source = txtImagePath.Text;
                    FileInfo info = new FileInfo(source);
                    string destination = @"C:\Users\Gabriella\source\repos\ThriffUI\Images\" + System.IO.Path.GetFileName(source);

                    info.CopyTo(destination);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void LoadProductForEditing(int productId)
        {
            isEditMode = true;
            currentProductId = productId;

            var product = productService.GetProductById(productId);
            if (product != null)
            {
                txtProductName.Text = product.Name;
                txtPrice.Text = product.Price.ToString();
                txtDescription.Text = product.Description;
                txtImagePath.Text = product.ImagePath;

                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    productImage.Source = new BitmapImage(new Uri(product.ImagePath));
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtProductName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) || string.IsNullOrWhiteSpace(txtImagePath.Text) || productImage.Source == null)
                {
                    MessageBox.Show("Please fill all fields and upload an image.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!double.TryParse(txtPrice.Text, out double price))
                {
                    MessageBox.Show("Invalid price. Please enter a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var product = new Product
                {
                    Name = txtProductName.Text,
                    Price = double.Parse(txtPrice.Text),
                    ImagePath = txtImagePath.Text,
                    Description = txtDescription.Text
                };

                if (isEditMode)
                {
                    productService.UpdateProduct(product);  // Update existing product
                    MessageBox.Show("Product updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    productService.AddProduct(product);     // Add new product
                    MessageBox.Show("Product added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                // Navigasi kembali ke halaman utama seller
                (Application.Current.MainWindow as MainWindow)?.NavigateToSellerHome();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow)?.NavigateToSellerHome();
        }
    }
}
