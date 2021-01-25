﻿using System;
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
            if ((txtName.Text == string.Empty)|| (txtAddress.Text == string.Empty)|| (txtPhoneNo.Text == string.Empty))
            {
                MessageBox.Show("Can not have empty records", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //int phoneNo;
            //if(!int.TryParse(txtPhoneNo.Text, out phoneNo))
            //{
            //    MessageBox.Show("Phone number should be numbers", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
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