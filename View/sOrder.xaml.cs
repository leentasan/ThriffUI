using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ThriffSignUp.Model;

namespace ThriffSignUp.View
{
    /// <summary>
    /// Interaction logic for sOrder.xaml
    /// </summary>
    public partial class sOrder : UserControl
    {
        public ObservableCollection<Order> NewOrders { get; set; } = new ObservableCollection<Order>();
        public ObservableCollection<Order> OnProcessOrders { get; set; } = new ObservableCollection<Order>();
        public ObservableCollection<Order> FinishedOrders { get; set; } = new ObservableCollection<Order>();

        public sOrder()
        {
            InitializeComponent();
            NewOrders.Add(new Order { OrderID = 1, ProductName = "Shirt", Quantity = 2, TotalPrice = 40, Status = "New" });

            // Bind data to ListViews
            NewOrdersList.ItemsSource = NewOrders;
            OnProcessOrdersList.ItemsSource = OnProcessOrders;
            FinishedOrdersList.ItemsSource = FinishedOrders;
        }
        private void AcceptOrder_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.CommandParameter as Order; // Replace with your actual model class

            if (order != null)
            {
                // Move order to On Process status
                order.Status = "On Process";
                MessageBox.Show($"Order {order.OrderID} accepted!");

                // Refresh the lists (implement appropriate methods)
                RefreshOrderLists();
            }
        }

        private void RejectOrder_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.CommandParameter as Order;

            if (order != null)
            {
                MessageBox.Show($"Order {order.OrderID} rejected!");

                // Remove the order or update status (implement appropriate methods)
                RemoveOrder(order);
            }
        }

        private void MarkAsDelivered_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.CommandParameter as Order;

            if (order != null)
            {
                // Move order to Finished status
                order.Status = "Finished";
                MessageBox.Show($"Order {order.OrderID} marked as delivered!");

                // Refresh the lists (implement appropriate methods)
                RefreshOrderLists();
            }
        }
        private void RemoveOrder(Order order)
        {
            if (NewOrders.Contains(order))
                NewOrders.Remove(order);
            else if (OnProcessOrders.Contains(order))
                OnProcessOrders.Remove(order);

            RefreshOrderLists();
        }

        // Refresh methods to reload the list views
        private void RefreshOrderLists()
        {
            // Force the ListView bindings to refresh
            NewOrdersList.ItemsSource = null;
            NewOrdersList.ItemsSource = NewOrders;

            OnProcessOrdersList.ItemsSource = null;
            OnProcessOrdersList.ItemsSource = OnProcessOrders;

            FinishedOrdersList.ItemsSource = null;
            FinishedOrdersList.ItemsSource = FinishedOrders;
        }
        private void BackArrow_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Content = new bHome();
            }
        }

    }
}
