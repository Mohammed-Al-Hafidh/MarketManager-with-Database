using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for NewCustomer.xaml
    /// </summary>
    public partial class NewCustomer : Window
    {
        public NewCustomer()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            
            AddCustomer();
        }

        private void AddCustomer()
        {
            if ((txtName.Text == string.Empty) || (txtAddress.Text == string.Empty) || (txtPhoneNo.Text == string.Empty))
            {
                MessageBox.Show("Can not have empty records", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string pattern = @"\d{10}";
            Regex rg = new Regex(pattern);
            if (!rg.IsMatch(txtPhoneNo.Text))
            {
                MessageBox.Show("Phone number should be only 10 digits", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Customer customer = new Customer
            {
                Name = txtName.Text,
                Address = txtAddress.Text,
                PhoneNumber = txtPhoneNo.Text
            };
            Globals.ctx.Customers.Add(customer);
            Globals.ctx.SaveChanges();
            DialogResult = true;
        }
    }
}
