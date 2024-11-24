using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ThriffSignUp.Model;

namespace ThriffSignUp.View
{
    public partial class bHome : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        private ObservableCollection<Product> _originalProducts = new ObservableCollection<Product>();

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly string connString = "Host=localhost;Port=5432;Database=thriff;Username=postgres;Password=della2908";

        public bHome()
        {
            InitializeComponent();
            DataContext = this;
            LoadProducts();
        }

        private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryFilter.SelectedItem is string selectedCategory)
            {
                // Debugging: Tampilkan nilai kategori yang dipilih
                Console.WriteLine($"Selected Category: {selectedCategory}");

                Products.Clear(); // Bersihkan produk sebelum menambahkan

                if (selectedCategory == "All")
                {
                    foreach (var product in _originalProducts)
                    {
                        Products.Add(product);
                    }
                }
                else
                {
                    foreach (var product in _originalProducts.Where(p =>
                        string.Equals(p.Category, selectedCategory, StringComparison.OrdinalIgnoreCase)))
                    {
                        Products.Add(product);
                    }
                }
            }
        }


        private void LoadProducts()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT productid, name, category, image_path, price FROM product";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            Products.Clear();
                            _originalProducts.Clear();
                            while (reader.Read())
                            {
                                var product = new Product
                                {
                                    ProductId = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Category = reader.IsDBNull(2) || string.IsNullOrWhiteSpace(reader.GetString(2))
                                                ? "Unknown"
                                                : reader.GetString(2).Trim(),
                                    ImagePath = reader.IsDBNull(3) || string.IsNullOrEmpty(reader.GetString(3))
                                                ? "/Images/ILUSTRASI.png"
                                                : reader.GetString(3),
                                    Price = (double)reader.GetDecimal(4)
                                };

                                Products.Add(product);
                                _originalProducts.Add(product);
                            }
                        }
                    }
                }

                // Isi kategori unik ke CategoryFilter
                var uniqueCategories = _originalProducts
                    .Select(p => p.Category)
                    .Distinct()
                    .ToList();
                uniqueCategories.Insert(0, "All");

                // Bersihkan item sebelumnya sebelum menetapkan ItemsSource
                CategoryFilter.Items.Clear();
                CategoryFilter.ItemsSource = uniqueCategories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void Product_Clicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ProductList.SelectedItem is Product selectedProduct)
            {
                var detailView = new DetailProduct
                {
                    ProductId = selectedProduct.ProductId // Set ProductId
                };

                var parent = this.Parent as ContentControl;
                if (parent != null)
                {
                    parent.Content = detailView;
                }
            }
            else
            {
                MessageBox.Show("No product selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox searchBox)
            {
                string searchText = searchBox.Text.ToLower();

                // Jika teks pencarian kosong, tampilkan semua produk
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    Products.Clear();
                    foreach (var product in _originalProducts)
                    {
                        Products.Add(product);
                    }
                    return;
                }

                // Filter produk berdasarkan teks pencarian
                Products.Clear();
                foreach (var product in _originalProducts.Where(p => p.Name.ToLower().Contains(searchText)))
                {
                    Products.Add(product);
                }
            }
        }


    }
}
