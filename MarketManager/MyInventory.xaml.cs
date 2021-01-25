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
    /// Interaction logic for MyInventory.xaml
    /// </summary>
    public partial class MyInventory : Window
    {
        public MyInventory()
        {
            InitializeComponent();
            RefreshAll();
        }
        public void LoadData()
        {
            lvProducts.ItemsSource = Globals.ctx.Products.ToList();
        }
        public void RefreshAll()
        {
            LoadData();
            lvProducts.Items.Refresh();
            lvProducts.UnselectAll();
            btnUpdateProduct.IsEnabled = false;
            txtProductName.IsEnabled = false;
            txtProductPrice.IsEnabled = false;
            txtQuantity.IsEnabled = false;
        }

        private void lvProducts_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                lvProducts.UnselectAll();
            RefreshAll();
        }      

        private void btnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            Product productToBeUpdated = (Product)lvProducts.SelectedItem;
            productToBeUpdated.ProductName = txtProductName.Text;
            int quantity;
            double price;
            if(!double.TryParse(txtProductPrice.Text,out price))
            {
                MessageBox.Show("Price should be a double value", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            productToBeUpdated.Price = price;
            if (!int.TryParse(txtQuantity.Text, out quantity))
            {
                MessageBox.Show("Quantity should be an integer value", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            productToBeUpdated.Inventory.Quantity = quantity;
            RefreshAll();
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void lvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvProducts.SelectedIndex == -1)
                return;
            btnUpdateProduct.IsEnabled = true;
            btnUpdateProduct.IsEnabled = true;
            txtProductName.IsEnabled = true;
            txtProductPrice.IsEnabled = true;
            txtQuantity.IsEnabled = true;

            Product productToBeUpdated = (Product)lvProducts.SelectedItem;
            txtProductName.Text = productToBeUpdated.ProductName;
            txtProductPrice.Text = productToBeUpdated.Price.ToString();
            txtQuantity.Text = productToBeUpdated.Quantity.ToString();
            Globals.ctx.SaveChanges();

            
        }
    }
}
