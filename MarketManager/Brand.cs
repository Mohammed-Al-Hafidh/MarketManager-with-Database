using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;

namespace MarketManager
{
    public class Brand
    {
        //public Brand()
        //{
        //    this.Products = new HashSet<Product>();
        //}
        [Key]
        [Required]
        public int IdBrand { get; set; }
        [StringLength(50)]
        public string BrandName { get; set; }
        //Product
        public ICollection<Product> Product { get; set; }
    }
}
