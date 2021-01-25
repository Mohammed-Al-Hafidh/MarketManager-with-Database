using System.Windows.Input;
using System.ComponentModel.DataAnnotations;

namespace MarketManager
{
    public class Inventory
    {
        [Key]
        [Required]
        public int IdInventory { get; set; }
        public int Quantity { get; set; }
        //Product
        public virtual Product Product { get; set; }


    }
}
