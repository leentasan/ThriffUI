﻿using System.Windows;
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
    }
}
