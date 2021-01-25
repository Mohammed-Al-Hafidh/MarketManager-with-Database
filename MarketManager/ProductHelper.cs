using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager
{
    public partial class Product
    {
        [NotMapped]
        public int Quantity { get => Inventory.Quantity; }
        [NotMapped]
        public string BrandName { get => brand.BrandName; }
        
    }
}
