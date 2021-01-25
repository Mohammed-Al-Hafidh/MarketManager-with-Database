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

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MarketManager
{
    /// <summary>
    /// Interaction logic for NewProduct.xaml
    /// </summary>
    public partial class NewProduct : Window
    {
        public Brand brand;
        byte[] ImageData;
        public NewProduct(Brand currentBrand)
        {
            InitializeComponent();
            brand = currentBrand;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if ((txtName.Text == string.Empty) || (txtPrice.Text == string.Empty) || (txtQuantity.Text == string.Empty))
            {
                MessageBox.Show("Can not have empty records", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            double price;
            if (!double.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Price should be a double", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int quantity;
            if (!int.TryParse(txtQuantity.Text, out quantity))
            {
                MessageBox.Show("Quantity should be an integer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Product product = new Product
            {
                ProductName = txtName.Text,
                Price = price,
                ProductImage = ImageData,
                Inventory = new Inventory { Quantity = quantity },
                IdBrand = brand.IdBrand
            };

            Globals.ctx.Products.Add(product);
            Globals.ctx.SaveChanges();
            DialogResult = true;
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
    }
}
