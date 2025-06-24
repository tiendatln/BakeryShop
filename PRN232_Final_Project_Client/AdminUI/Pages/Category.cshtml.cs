using DTOs.CategoryDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Interfaces;
using Service.Services;

namespace PRN232_Final_Project_Client.Pages.Categories
{
    public class CategoryModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CategoryModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public List<ReadCategoryDTO> Categories { get; set; } = new();

        [BindProperty]
        public CreateCategoryDTO NewCategory { get; set; } = new CreateCategoryDTO();

        [BindProperty]
        public UpdateCategoryDTO EditCategory { get; set; } = new();

        public async Task OnGetAsync()
        {
            Categories = await _categoryService.GetAllCategoriesAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            // Xử lý Description - đặt empty string nếu null
            if (NewCategory != null && string.IsNullOrEmpty(NewCategory.Description))
            {
                NewCategory.Description = string.Empty;
            }

            // Xóa tất cả lỗi validation và validate lại manual
            ModelState.Clear();

            // Validate manual chỉ cho CategoryName
            if (NewCategory == null)
            {
                ModelState.AddModelError("", "Category data is required");
                return await ReloadPage();
            }

            if (string.IsNullOrWhiteSpace(NewCategory.CategoryName))
            {
                ModelState.AddModelError("NewCategory.CategoryName", "Category name is required");
                return await ReloadPage();
            }

            if (NewCategory.CategoryName.Length > 50)
            {
                ModelState.AddModelError("NewCategory.CategoryName", "Category name cannot exceed 50 characters");
                return await ReloadPage();
            }

            // Validate Description nếu có giá trị
            if (!string.IsNullOrEmpty(NewCategory.Description) && NewCategory.Description.Length > 255)
            {
                ModelState.AddModelError("NewCategory.Description", "Description cannot exceed 255 characters");
                return await ReloadPage();
            }

            var token = HttpContext.Session.GetString("token");
            var result = await _categoryService.CreateCategoryAsync(NewCategory, token);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to create category.");
                return await ReloadPage();
            }

            return RedirectToPage("/Category");
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            // Xử lý Description cho Update
            if (EditCategory != null && string.IsNullOrEmpty(EditCategory.Description))
            {
                EditCategory.Description = string.Empty;
            }

            // Validate manual cho Update
            ModelState.Clear();

            if (EditCategory == null)
            {
                ModelState.AddModelError("", "Category data is required");
                return await ReloadPage();
            }

            if (EditCategory.CategoryID <= 0)
            {
                ModelState.AddModelError("EditCategory.CategoryID", "Category ID is required");
                return await ReloadPage();
            }

            if (string.IsNullOrWhiteSpace(EditCategory.CategoryName))
            {
                ModelState.AddModelError("EditCategory.CategoryName", "Category name is required");
                return await ReloadPage();
            }

            if (EditCategory.CategoryName.Length > 50)
            {
                ModelState.AddModelError("EditCategory.CategoryName", "Category name cannot exceed 50 characters");
                return await ReloadPage();
            }

            if (!string.IsNullOrEmpty(EditCategory.Description) && EditCategory.Description.Length > 255)
            {
                ModelState.AddModelError("EditCategory.Description", "Description cannot exceed 255 characters");
                return await ReloadPage();
            }

            var token = HttpContext.Session.GetString("token");
            var result = await _categoryService.UpdateCategoryAsync(EditCategory.CategoryID, EditCategory, token);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to update category.");
                return await ReloadPage();
            }

            return RedirectToPage("/Category");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var token = HttpContext.Session.GetString("token");
            var result = await _categoryService.DeleteCategoryAsync(id, token);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete category.");
                return await ReloadPage();
            }

            return RedirectToPage("/Category");
        }

        private async Task<IActionResult> ReloadPage()
        {
            Categories = await _categoryService.GetAllCategoriesAsync();
            return Page();
        }
    }
}