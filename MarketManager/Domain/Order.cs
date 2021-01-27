using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;

namespace MarketManager
{
    public class Order
    {
        public Order()
        {
            this.Products = new HashSet<Product>();
        }
        [Key]
        [Required]
        public int IdOrder { get; set; }
        public double TotalPrice { get; set; }
        public string Orderdate { get; set; }
        //Product
        public ICollection<Product> Products { get; set; }
        //Customer
        public int IdCustomer { get; set; }
        public Customer Customer { get; set; }
    }
}
