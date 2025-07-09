using System.ComponentModel.DataAnnotations;

namespace DTOs.CategoryDTO

{
    public class CreateCategoryDTO
    {

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
        public string Description { get; set; } = string.Empty;

    }
}