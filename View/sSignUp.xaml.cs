﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThriffSignUp.View
{
    /// <summary>
    /// Interaction logic for sSignUp.xaml
    /// </summary>
    public partial class sSignUp : UserControl
    {
        public sSignUp()
        {
            InitializeComponent();
        }
        private void GoToLogin(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow)?.NavigateToSellerLogIn();
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
