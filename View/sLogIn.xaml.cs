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
    /// Interaction logic for sLogIn.xaml
    /// </summary>
    public partial class sLogIn : UserControl
    {
        public sLogIn()
        {
            InitializeComponent();
        }
        private void GoToSignUp(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow)?.NavigateToSellerSignUp();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
