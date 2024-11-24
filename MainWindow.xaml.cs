using System.Windows;
using System;
using ThriffSignUp.View;

namespace ThriffSignUp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadWelcomeView();
        }

        private void LoadWelcomeView()
        {
            try
            {
                var welcomeView = new View.WelcomeView();
                this.Content = welcomeView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load WelcomeView: {ex.Message}");
            }
        }

        public void NavigateToBuyerLogin()
        {
            try
            {
                var bLogIn = new View.bLogIn();
                this.Content = bLogIn;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load LoginView: {ex.Message}");
            }
        }

        public void NavigateToSignUpPage()
        {
            try
            {
                var bSignUp = new View.bSignUp();
                this.Content = bSignUp;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load SignUpView: {ex.Message}");
            }
        }

        public void NavigateToHomeBuyer()
        {
            try
            {
                // Buat instance dari bHome
                var homeView = new View.bHome();
                this.Content = homeView; // Ganti konten utama ke bHome
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load HomeView: {ex.Message}", "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void NavigateToSellerSignUp()
        {
            try
            {
                var signUp = new View.sSignUp();
                this.Content = signUp;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load SellerSignUp: {ex.Message}");
            }
        }
        public void NavigateToSellerLogIn()
        {
            try
            {
                var logIn = new View.sLogIn();
                this.Content = logIn;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load SellerLogIn: {ex.Message}");
            }
        }
        public void NavigateToSellerHome()
        {
            try
            {
                var sellHome = new View.sHome();
                this.Content = sellHome;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load SellerHome: {ex.Message}");
            }
        }
        public void NavigateToProductsEdit()
        {
            try
            {
                var product = new View.sProduct();
                this.Content = product;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Failed to load Product: {e.Message}");
            }
        }
        public void NavigateToProductsEdit(int productId)
        {
            // Create an instance of the sProduct page
            var productPage = new sProduct();

            // Pass the product ID to load the product data for editing
            productPage.LoadProductForEditing(productId);

            // Set the ContentControl's content to the productPage
            ContentArea.Content = productPage;
        }


        public void NavigateToOrders()
        {
            try
            {
                var orders = new View.sOrder();
                this.Content = orders;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Failed to load Orders: {e.Message}");
            }
        }
    }
}
