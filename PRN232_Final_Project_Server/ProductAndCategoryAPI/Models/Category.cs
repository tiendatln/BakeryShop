using System.ComponentModel.DataAnnotations;

namespace ProductAndCategoryAPI.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
