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

namespace MarketManager
{
    /// <summary>
    /// Interaction logic for OrderDetails.xaml
    /// </summary>
    public partial class OrderDetails : Window
    {
        Order order;
        List<Product> products = new List<Product>();
        List<Order> productsorders = new List<Order>();
        public OrderDetails(Order currentOrder)
        {
            InitializeComponent();
            order = currentOrder;
            Refresh();
        }
        public void LoadData()
        {          

            lvOrder.ItemsSource = order.Products.ToList<Product>();
            lblCustomerId.Content = order.Customer.IdCustomer;
            lblCustomerName.Content = order.Customer.Name;
            lblOrderId.Content = order.IdOrder;
            lblTotalPrice.Content = order.TotalPrice;
            lvOrder.Items.Refresh();
            btnDelete.IsEnabled = false;

        }
        public void Refresh()
        {

            LoadData();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrder.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select one item", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Product productToBeDeleted = (Product)lvOrder.SelectedItem;
            order.TotalPrice -= productToBeDeleted.Price;
            order.Products.Remove(productToBeDeleted);
            Globals.ctx.SaveChanges();
            LoadData();
            
            
        }

        private void lvOrder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                lvOrder.UnselectAll();
        }

        private void lvOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDelete.IsEnabled = true;
        }
    }
}
