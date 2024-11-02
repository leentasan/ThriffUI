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
    public partial class WelcomeView : UserControl
    {
        public WelcomeView()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (roleComboBox.SelectedItem is ComboBoxItem selectedRole)
            {
                string role = selectedRole.Content.ToString();

                if (role == "Seller")
                {
                    (Application.Current.MainWindow as MainWindow)?.NavigateToSellerLogIn();
                }
                else if (role == "Buyer")
                {
                    (Application.Current.MainWindow as MainWindow)?.NavigateToBuyerLogin();
                }
            }
            else
            {
                MessageBox.Show("Please select a role to continue.", "Role Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void roleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

