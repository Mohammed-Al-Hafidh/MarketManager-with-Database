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
    /// Interaction logic for NewBrand.xaml
    /// </summary>
    public partial class NewBrand : Window
    {
        public NewBrand()
        {
            InitializeComponent();
        }

        private void btnAddBrand_Click(object sender, RoutedEventArgs e)
        {            
            AddBrand();
        }

        private void AddBrand()
        {
            if (txtAddBrand.Text == string.Empty)
            {
                MessageBox.Show("Brand name can not be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Globals.ctx.Brands.Add(new Brand { BrandName = txtAddBrand.Text });
            Globals.ctx.SaveChanges();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
