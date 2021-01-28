using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;

namespace MarketManager
{
    public class Brand
    {
        
        [Key]
        [Required]
        public int IdBrand { get; set; }
        [StringLength(50)]
        public string BrandName { get; set; }
        //Product
        public ICollection<Product> Product { get; set; }
    }
}
