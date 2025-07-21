using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAndCategoryAPI.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        public string ImageURL { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedDate { get; set; } 

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public Category Category { get; set; }
    }
}
