using System.Windows;
using System;

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

        public void NavigateToLoginPage()
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
    }
}
