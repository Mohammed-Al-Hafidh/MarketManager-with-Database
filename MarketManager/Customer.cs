using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;

namespace MarketManager
{
    public class Customer
    {
        [Key]
        [Required]
        public int IdCustomer { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [StringLength(50)]
        public string Address { get; set; }
        //Order
        public ICollection<Order> Order { get; set; }
    }
}
