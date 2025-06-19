using System.ComponentModel.DataAnnotations;

namespace ProductAndCategoryAPI.DTOs
{
    public class UpdateCategoryDTO
    {
        [Required]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
        public string CategoryName { get; set; }

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
        public string Description { get; set; }
    }
}
