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
                var loginView = new View.LoginView();
                this.Content = loginView;
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
                var signUpView = new View.SignUpView();
                this.Content = signUpView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load SignUpView: {ex.Message}");
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
        public void NavigateToProductsEdit(int? productId = null)
        {
            var productControl = new sProduct();

            if (productId.HasValue)
            {
                productControl.LoadProductForEditing(productId.Value);
            }

            Content = productControl;
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
