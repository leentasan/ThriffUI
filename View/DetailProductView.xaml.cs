using System.Windows;
using ThriffSignUp.Model;

public partial class DetailProductView : Window
{
    public Product ProductDetail { get; set; }

    public DetailProductView(Product selectedProduct = null)
    {
        InitializeComponent();
        if (selectedProduct == null)
        {
            // Jika tidak ada produk yang dipilih, gunakan dummy data
            ProductDetail = new Product
            {
                Name = "Vintage Jacket",
                Price = 45.99,
                ImagePath = "/Images/ILUSTRASI.png",
                Description = "This is a vintage jacket made from premium materials."
            };
        }
        else
        {
            // Gunakan produk yang dipilih
            ProductDetail = selectedProduct;
        }
        DataContext = ProductDetail;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void btnMinimize_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }
}
