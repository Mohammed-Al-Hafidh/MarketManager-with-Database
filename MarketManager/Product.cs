using System.Windows.Input;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MarketManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class Product
    {
        public Product()
        {
            this.Orders = new HashSet<Order>();
            
        }
        [Key]
        [Required]
        public int IdProduct { get; set; }
        [StringLength(50)]
        public string ProductName { get; set; }        
        public double Price { get; set; }
        public byte[] ProductImage { get; set; }
        //Brand        
        public int IdBrand { get; set; }
        public Brand brand { get; set; }
        //Order
        public virtual ICollection<Order> Orders { get; set; }
        //Inventory
        public virtual Inventory Inventory { get; set; }
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", IdProduct, ProductName, Price, Inventory.Quantity);
        }


    }
}
