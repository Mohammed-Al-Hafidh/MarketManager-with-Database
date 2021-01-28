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
        Order selectedOrder;
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
            AddProduct();
        }
        public void AddProduct()
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
            CheckResult(result);
        }

        private void btnAddBrand_Click(object sender, RoutedEventArgs e)
        {            
            
            AddBrand();
        }

        private void AddBrand()
        {
            NewBrand newBrand = new NewBrand();
            newBrand.Owner = this;
            bool? result = newBrand.ShowDialog();
            CheckResult(result);
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {            
            AddOrder();
            RefreshListViews();
        }

        private void AddOrder()
        {
            if ((lvProducts.SelectedIndex == -1) || (lvCustomers.SelectedIndex == -1))
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
                    MessageBox.Show("You have 0 of " + p.ProductName + " in the inventory"
                        , "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            foreach (Product p in lvProducts.SelectedItems)
            {
                order.Products.Add(p);
                order.Orderdate = dtpOrderDate.Value.ToString();
                order.TotalPrice += p.Price;
                p.Inventory.Quantity -= 1;
            }

            Globals.ctx.Orders.Add(order);
            Globals.ctx.SaveChanges();
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {            
            
            AddNewCustomer();
        }

        private void AddNewCustomer()
        {
            NewCustomer newCustomer = new NewCustomer();
            newCustomer.Owner = this;
            bool? result = newCustomer.ShowDialog();
            CheckResult(result);
           
        }
        private void CheckResult(bool? result)
        {
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

        private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {            
            DeleteCustomer();
            RefreshListViews();
        }

        private void DeleteCustomer()
        {
            if (lvCustomers.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select a customer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Customer customerToBeDeleted = (Customer)lvCustomers.SelectedItem;
            Globals.ctx.Customers.Remove(customerToBeDeleted);
            Globals.ctx.SaveChanges();
        }

        private void btnDeleteBrand_Click(object sender, RoutedEventArgs e)
        {            
            DeleteBrand();
            RefreshListViews();
        }

        private void DeleteBrand()
        {
            if (lvBrands.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select a customer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Brand brandToBeDeleted = (Brand)lvBrands.SelectedItem;
            Globals.ctx.Brands.Remove(brandToBeDeleted);
            Globals.ctx.SaveChanges();
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
            
            LvProductsSelectionChanged();
        }

        private void LvProductsSelectionChanged()
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
                BitmapImage bi = Util.ToImage(ImageData);

                brush.ImageSource = bi;
                btnAddProductImage.Content = "";
                btnAddProductImage.Background = brush;
            }
        }

        private void btnUpdateProductInventory_Click(object sender, RoutedEventArgs e)
        {            
            
            UpdateProductInventory();
            RefreshAllInventory();
            RefreshListViews();
        }

        private void UpdateProductInventory()
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
            //Update total price in orders
            foreach (var order in productToBeUpdated.Orders)
            {
                order.TotalPrice = 0;
                foreach(var product in order.Products)
                {
                    order.TotalPrice += product.Price;
                }
            }
            Globals.ctx.SaveChanges();
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
            
            AddProductImage();
        }

        private void AddProductImage()
        {
            FileStream fs;
            BinaryReader br;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a picture";
            openFileDialog.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                btnAddProductImage.Content = "";

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
            DeleteProduct();
            RefreshAllInventory();
            RefreshListViews();
        }

        private void DeleteProduct()
        {
            if (lvProductsInventory.SelectedIndex == -1)
                return;
            Product productToBeDeleted = (Product)lvProductsInventory.SelectedItem;
            Globals.ctx.Products.Remove(productToBeDeleted);
            Globals.ctx.SaveChanges();
        }
        //End Inventory Tab//

        //Start Orders Tab//        
        public void LoadDataOrderMain()
        {
            lvOrdrs.ItemsSource = Globals.ctx.Orders.ToList();
            lvOrderMain.ItemsSource = selectedOrder.Products.ToList<Product>();
            
            
            lvOrderMain.Items.Refresh();
            btnDeleteMain.IsEnabled = false;
            btnExportOrder.IsEnabled = false;
        }
        public void RefreshOrderMain()
        {
            LoadDataOrderMain();
            btnDeleteOrderMain.IsEnabled = false;
            lvOrdrs.UnselectAll();
            lvOrderMain.ItemsSource = null;
            lblCustomerIdMain.Content = "";
            lblCustomerNameMain.Content = "";
            lblOrderIdMain.Content = "";
            lblTotalPriceMain.Content = "";
            lblOrderDateMain.Content = "";                        
        }
        private void lvOrderMain_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
            {
                lvOrderMain.UnselectAll();
                btnDeleteMain.IsEnabled = false;
                btnExportOrder.IsEnabled = false;
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
            DeleteProductFromOrder();           
            RefreshOrderMain();
        }

        private void DeleteProductFromOrder()
        {
            if (lvOrderMain.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select one item", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Product productToBeDeleted = (Product)lvOrderMain.SelectedItem;
            selectedOrder.TotalPrice -= productToBeDeleted.Price;
            //Adding the product back to inventory
            productToBeDeleted.Inventory.Quantity++;
            selectedOrder.Products.Remove(productToBeDeleted);            
            Globals.ctx.SaveChanges();
            LoadData();
        }

        private void lvOrdrs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {                        
            LvOrderSelectionChanged();
        }

        private void LvOrderSelectionChanged()
        {
            if (lvOrdrs.SelectedIndex == -1)
                return;

            lvOrderMain.IsEnabled = true;
            Order order = (Order)lvOrdrs.SelectedItem;
            List<Order> allOrders = Globals.ctx.Orders.Include("Products").ToList();
            selectedOrder = allOrders.Where(o => o.IdOrder == order.IdOrder).FirstOrDefault();

            lblCustomerIdMain.Content = selectedOrder.Customer.IdCustomer;
            lblCustomerNameMain.Content = selectedOrder.Customer.Name;
            lblOrderIdMain.Content = selectedOrder.IdOrder;
            lblTotalPriceMain.Content = selectedOrder.TotalPrice;
            lblOrderDateMain.Content = selectedOrder.Orderdate;

            LoadDataOrderMain();

            btnDeleteOrderMain.IsEnabled = true;
            btnExportOrder.IsEnabled = true;
        }

        private void btnDeleteOrderMain_Click(object sender, RoutedEventArgs e)
        {            
            DeleteOrder();
        }

        private void DeleteOrder()
        {
            if (lvOrdrs.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select one item", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Order orderToBeDeleted = (Order)lvOrdrs.SelectedItem;
            Globals.ctx.Orders.Remove(orderToBeDeleted);
            Globals.ctx.SaveChanges();
            RefreshOrderMain();
        }

        private void btnExportOrder_Click(object sender, RoutedEventArgs e)
        {
            ExportOrder();
        }

        private void ExportOrder()
        {
            if (lvOrdrs.SelectedIndex == -1)
                return;
            Order orderToBeSaved =(Order) lvOrdrs.SelectedItem;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV file (*.csv)|*.csv";
            dialog.Title = "Export to file";
            if (dialog.ShowDialog() == true)
            {
                string allData = "";

                allData = string.Format("Customer Name:,{0}", orderToBeSaved.Customer.Name) + "\n";
                allData += string.Format("Customer Id:,{0}", orderToBeSaved.Customer.IdCustomer) + "\n";
                allData += string.Format("Customer Address:,{0}", orderToBeSaved.Customer.Address) + "\n";
                allData += string.Format("Customer Phone No.:,{0}", orderToBeSaved.Customer.PhoneNumber) + "\n";
                allData += string.Format("Order Date:,{0}", orderToBeSaved.Orderdate) + "\n";
                allData += string.Format("Order Id:,{0}", orderToBeSaved.IdOrder) + "\n";
                allData +="\n";
                allData += string.Format("{0},{1},{2}","Brand Name","Product Name","Product Price" ) + "\n";
                allData += "\n";
                foreach (Product product in orderToBeSaved.Products)
                {                    
                    allData += string.Format("{0},{1},{2}",product.BrandName,product.ProductName,product.Price) +"\n";
                }
                allData += "\n";
                allData += string.Format("{0},{1},{2}","","Total Price:",orderToBeSaved.TotalPrice);
               
                File.WriteAllText(dialog.FileName, allData);
            }
            else
            {
                MessageBox.Show("File error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //End Orders Tab//
    }
}
