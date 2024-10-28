using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ThriffSignUp.Model;
using ThriffSignUp.View;

namespace ThriffSignUp.View
{
    public partial class HomeView : Window
    {
        public ObservableCollection<Product> Products { get; set; }

        public HomeView()
        {
            InitializeComponent();
            LoadDummyData();
            DataContext = this; // Set DataContext ke HomeView
        }

        private void LoadDummyData()
        {
            Products = new ObservableCollection<Product>
    {
            new Product { Name = "Vintage Jacket", Price = 45.99, ImagePath = "/Images/ILUSTRASI.png" },
            new Product { Name = "Retro Sneakers", Price = 30.49, ImagePath = "https://via.placeholder.com/150" },
            new Product { Name = "Classic Hat", Price = 15.99, ImagePath = "https://via.placeholder.com/150" }
    };

            MessageBox.Show($"Products Count: {Products.Count}");
        }


        private void Product_Clicked(object sender, MouseButtonEventArgs e)
        {
            //if (ProductList.SelectedItem is Product selectedProduct)
            //{
            //    DetailProductView detailView = new DetailProductView(selectedProduct);
            //    detailView.Show();
            //}
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void GoToCart_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Navigating to Cart...");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void ProductList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
