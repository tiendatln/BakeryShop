using System.ComponentModel.DataAnnotations;

namespace ProductAndCategoryAPI.DTOs
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Please Enter Name Product")]
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; }
        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryID { get; set; }
        [Required]
        public IFormFile ImageURL { get; set; } = default;
        public bool IsAvailable { get; set; } = true;
    }
}
