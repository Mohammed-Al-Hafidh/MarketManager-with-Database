using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace MarketManager
{


    public partial class MainWindow : Window
    {
        byte[] ImageData;
        public MainWindow()
        {
            InitializeComponent();

            Globals.ctx = new MarketManagerDatabaseContext();            
            LoadData();
        }
        public void LoadData()
        {
            lvProducts.ItemsSource = Globals.ctx.Products.ToList();
            lvBrands.ItemsSource = Globals.ctx.Brands.ToList();
            lvCustomers.ItemsSource = Globals.ctx.Customers.ToList();
            lvOrdrs.ItemsSource = Globals.ctx.Orders.ToList();
            //Inventory Tab//
            RefreshAllInventory();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (lvBrands.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select a brand", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }           

            Brand brand = (Brand)lvBrands.SelectedItem;
            NewProduct newProduct = new NewProduct(brand);
            newProduct.Owner = this;
            bool? result = newProduct.ShowDialog();
            

            if (result == true)
            {
                RefreshListViews();
            }
            else if (result == false)
            {
                MessageBox.Show("Request canceled", "Cancel", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void btnAddBrand_Click(object sender, RoutedEventArgs e)
        {            
            NewBrand newBrand = new NewBrand();
            newBrand.Owner = this;
            bool? result = newBrand.ShowDialog();

            if (result == true)
            {
                RefreshListViews();
            }
            else if (result == false)
            {
                MessageBox.Show("Request canceled", "Cancel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            if ((lvProducts.SelectedIndex == -1)||(lvCustomers.SelectedIndex==-1))
            {
                MessageBox.Show("You need to select a product and a customer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Customer customer = (Customer)lvCustomers.SelectedItem;                       
            Order order = new Order
            {                                     
                IdCustomer = customer.IdCustomer
            };
            foreach (Product p in lvProducts.SelectedItems)
            {
                if (p.Inventory.Quantity <= 0)
                {
                    MessageBox.Show("You have 0 of" + p.ProductName + " in the inventory", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
                foreach (Product p in lvProducts.SelectedItems)
            {
                order.Products.Add(p);
                //order.Orderdate = dpOrderDate.SelectedDate.ToString();
                order.Orderdate = dtpOrderDate.Value.ToString();
                order.TotalPrice += p.Price;
                p.Inventory.Quantity -= 1;
            }            
            
            Globals.ctx.Orders.Add(order);
            Globals.ctx.SaveChanges();
            RefreshListViews();
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {            
            NewCustomer newCustomer = new NewCustomer();
            newCustomer.Owner = this;
            bool? result = newCustomer.ShowDialog();            

            if (result == true)
            {
                RefreshListViews();
            }
            else if (result == false)
            {
                MessageBox.Show("Request canceled", "Cancel", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public void RefreshListViews()
        {
            LoadData();
            lvProducts.Items.Refresh();
            lvProducts.UnselectAll();

            lvBrands.Items.Refresh();
            lvBrands.UnselectAll();

            lvCustomers.Items.Refresh();
            lvCustomers.UnselectAll();

            lvOrdrs.Items.Refresh();
            lvOrdrs.UnselectAll();
        }

        private void lvProducts_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                lvProducts.UnselectAll();
        }

        private void lvBrands_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                lvBrands.UnselectAll();
        }

        private void lvCustomers_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                lvCustomers.UnselectAll();
        }

        private void lvOrdrs_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
            {
                lvOrdrs.UnselectAll();
                lvOrderMain.ItemsSource = null;
                lblCustomerIdMain.Content = "";
                lblCustomerNameMain.Content = "";
                lblOrderIdMain.Content = "";
                lblTotalPriceMain.Content = "";
                lblOrderDateMain.Content = "";
                btnDeleteOrderMain.IsEnabled = false;
            }
        }

        //private void btnShowOrderDetails_Click(object sender, RoutedEventArgs e)
        //{
        //    if (lvOrdrs.SelectedIndex == -1) 
        //    {
        //        MessageBox.Show("You need to select an order", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    Order order = (Order)lvOrdrs.SelectedItem;
        //    List<Order> allOrders = Globals.ctx.Orders.Include("Products").ToList();
        //    Order sendOrder = allOrders.Where(o => o.IdOrder == order.IdOrder).FirstOrDefault();            

        //    OrderDetails orderDetails = new OrderDetails(sendOrder);
        //    orderDetails.Owner = this;
        //    bool? result = orderDetails.ShowDialog();
        //    if (result == true)
        //    {
        //        RefreshListViews();
        //    }
        //    else if (result == false)
        //    {
        //        MessageBox.Show("Request canceled", "Cancel", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (lvCustomers.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select a customer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Customer customerToBeDeleted = (Customer)lvCustomers.SelectedItem;
            Globals.ctx.Customers.Remove(customerToBeDeleted);
            Globals.ctx.SaveChanges();
            RefreshListViews();
        }

        private void btnDeleteBrand_Click(object sender, RoutedEventArgs e)
        {
            if (lvBrands.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select a customer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Brand brandToBeDeleted = (Brand)lvBrands.SelectedItem;            
            Globals.ctx.Brands.Remove(brandToBeDeleted);
            Globals.ctx.SaveChanges();
            RefreshListViews();
        }

        private void btnShowInventory_Click(object sender, RoutedEventArgs e)
        {
            MyInventory myInventory = new MyInventory();
            myInventory.Owner = this;
            bool? result = myInventory.ShowDialog();
            
            if (result == true)
            {
                RefreshListViews();
            }
            else if (result == false)
            {
                MessageBox.Show("Request canceled", "Cancel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Start Inventory Tab//
        public void LoadDataInventory()
        {
            lvProductsInventory.ItemsSource = Globals.ctx.Products.ToList();
        }
        public void RefreshAllInventory()
        {
            LoadDataInventory();
            lvProductsInventory.Items.Refresh();
            lvProductsInventory.UnselectAll();
            btnUpdateProductInventory.IsEnabled = false;
            txtProductNameInventory.IsEnabled = false;
            txtProductNameInventory.Clear();
            txtProductPriceInventory.IsEnabled = false;
            txtProductPriceInventory.Clear();
            txtQuantityInventory.IsEnabled = false;
            txtQuantityInventory.Clear();
            btnAddProductImage.Content = "Add Image";
            btnAddProductImage.Background = null;
            btnDeleteProduct.IsEnabled = false;

        }
       

        private void lvProductsInventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvProductsInventory.SelectedIndex == -1)
                return;
            btnUpdateProductInventory.IsEnabled = true;
            btnUpdateProductInventory.IsEnabled = true;
            txtProductNameInventory.IsEnabled = true;
            txtProductPriceInventory.IsEnabled = true;
            txtQuantityInventory.IsEnabled = true;
            btnDeleteProduct.IsEnabled = true;

            Product productToBeUpdated = (Product)lvProductsInventory.SelectedItem;
            txtProductNameInventory.Text = productToBeUpdated.ProductName;
            txtProductPriceInventory.Text = productToBeUpdated.Price.ToString();
            txtQuantityInventory.Text = productToBeUpdated.Quantity.ToString();
            ImageData = productToBeUpdated.ProductImage;

            // Set button background
            if (ImageData != null)
            {
                ImageBrush brush = new ImageBrush();
                //BitmapImage bi = new BitmapImage(ImageData);/////////////////
                BitmapImage bi = Util.ToImage(ImageData);

                brush.ImageSource = bi;
                btnAddProductImage.Content = "";
                btnAddProductImage.Background = brush;
            }

        }
        

        private void btnUpdateProductInventory_Click(object sender, RoutedEventArgs e)
        {
            Product productToBeUpdated = (Product)lvProductsInventory.SelectedItem;
            productToBeUpdated.ProductName = txtProductNameInventory.Text;
            int quantity;
            double price;
            if (!double.TryParse(txtProductPriceInventory.Text, out price))
            {
                MessageBox.Show("Price should be a double value", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            productToBeUpdated.Price = price;
            if (!int.TryParse(txtQuantityInventory.Text, out quantity))
            {
                MessageBox.Show("Quantity should be an integer value", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            productToBeUpdated.ProductImage = ImageData;
            productToBeUpdated.Inventory.Quantity = quantity;
            Globals.ctx.SaveChanges();
            RefreshAllInventory();
            RefreshListViews();
        }

        private void lvProductsInventory_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                lvProductsInventory.UnselectAll();
            RefreshAllInventory();
        }
        private void btnAddProductImage_Click(object sender, RoutedEventArgs e)
        {
            FileStream fs;
            BinaryReader br;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a picture";
            openFileDialog.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                //imgProductImage.Source= new BitmapImage(new Uri(openFileDialog.FileName));


                Image img = new Image();
                img.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                btnAddProductImage.Content = "";
                //btnImage.Content = "";

                // Set button background
                ImageBrush brush = new ImageBrush();
                BitmapImage bi = new BitmapImage(new Uri(openFileDialog.FileName));
                brush.ImageSource = bi;
                ///Image.Background = brush;
                btnAddProductImage.Background = brush;

                //Image to byte[] to save it in database
                string FileName = openFileDialog.FileName;
                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                ImageData = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lvProductsInventory.SelectedIndex == -1)
                return;
            Product productToBeDeleted = (Product)lvProductsInventory.SelectedItem;
            Globals.ctx.Products.Remove(productToBeDeleted);            
            Globals.ctx.SaveChanges();
            RefreshAllInventory();
            RefreshListViews();
        }
        //End Inventory Tab//

        //Start Orders Tab//
        Order selectedOrder;
        public void LoadDataOrderMain()
        {
            lvOrdrs.ItemsSource = Globals.ctx.Orders.ToList();
            lvOrderMain.ItemsSource = selectedOrder.Products.ToList<Product>();
            lblCustomerIdMain.Content = selectedOrder.Customer.IdCustomer;
            lblCustomerNameMain.Content = selectedOrder.Customer.Name;
            lblOrderIdMain.Content = selectedOrder.IdOrder;
            lblTotalPriceMain.Content = selectedOrder.TotalPrice;
            lblOrderDateMain.Content = selectedOrder.Orderdate;
            lvOrderMain.Items.Refresh();
            btnDeleteMain.IsEnabled = false;

        }
        public void RefreshOrderMain()
        {
            LoadDataOrderMain();
        }
        private void lvOrderMain_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
            {
                lvOrderMain.UnselectAll();
                btnDeleteMain.IsEnabled = false;
            }
            
        }

        private void lvOrderMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvOrderMain.SelectedIndex == -1)
                return;
            btnDeleteMain.IsEnabled = true;
        }

        private void btnDeleteMain_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrderMain.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select one item", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Product productToBeDeleted = (Product)lvOrderMain.SelectedItem;
            selectedOrder.TotalPrice -= productToBeDeleted.Price;
            selectedOrder.Products.Remove(productToBeDeleted);
            Globals.ctx.SaveChanges();
            LoadDataOrderMain();
            RefreshOrderMain();
        }

        private void lvOrdrs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (lvOrdrs.SelectedIndex == -1)            
                return;
            
            lvOrderMain.IsEnabled = true;
            Order order = (Order)lvOrdrs.SelectedItem;
            List<Order> allOrders = Globals.ctx.Orders.Include("Products").ToList();
            selectedOrder = allOrders.Where(o => o.IdOrder == order.IdOrder).FirstOrDefault();
            LoadDataOrderMain();

            btnDeleteOrderMain.IsEnabled = true;
        }

        private void btnDeleteOrderMain_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrdrs.SelectedIndex == -1) 
            {
                MessageBox.Show("You need to select one item", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Order orderToBeDeleted = (Order)lvOrdrs.SelectedItem;
            Globals.ctx.Orders.Remove(orderToBeDeleted);
            Globals.ctx.SaveChanges();
            btnDeleteOrderMain.IsEnabled = false;
            lvOrdrs.UnselectAll();
            lvOrderMain.ItemsSource = null;
            lblCustomerIdMain.Content = "";
            lblCustomerNameMain.Content = "";
            lblOrderIdMain.Content = "";
            lblTotalPriceMain.Content = "";
            lblOrderDateMain.Content = "";
            lvOrdrs.ItemsSource = Globals.ctx.Orders.ToList();
            lvOrdrs.Items.Refresh();

        }

        


        //End Orders Tab//


    }
}
