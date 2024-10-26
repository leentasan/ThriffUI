using System;
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
using System.Windows.Shapes;


namespace ThriffSignUp.View
{
    public partial class WelcomeView : Window
    {
        public WelcomeView()
        {
            InitializeComponent();
        }

        // Event handler for the Next button


        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to LoginView
            (Application.Current.MainWindow as MainWindow)?.NavigateToLoginPage();
        }
    }
}

