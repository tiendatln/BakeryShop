using Azure;
using DTOs.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Interfaces;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace AdminUI.Pages.Admin
{
    public class ProductManagementModel : PageModel
    {
        private readonly IProductService _productService;

        [BindProperty(SupportsGet = true)]
        public List<ReadProductDTO> readProducts { get; set; } = new List<ReadProductDTO>();

        public ProductManagementModel(IProductService productService)
        {
            _productService = productService;
        }

        public async Task OnGet()
        {
            HttpContext.Session.SetString("token"
                , "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiTEUgTkdVRU4gVElFTiBEQVQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0aWVuZGF0bGUyMjEyQGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzUwNzc5NDcwLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDA5IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzExMiJ9._TzuaJ2H-6Vs-YAl7ranm41DqvYIw9BEQg-BujTzNvg"); // Set the active page in session
        }

        public async Task<IActionResult> OnGetAllProductAsync([FromQuery] int page)
        {
            var skip = (page - 1) * 10; // Calculate the number of products to skip based on the current page\

            var allproducts = await _productService.GetAllProductsAsync();
            var totalPages = (int)Math.Ceiling(allproducts.Count / 10f);

            var products = await _productService.GetProductPage(10, skip);

            if (products == null || !products.Any())
            {
                return new JsonResult(new { success = false, message = "No products found." });
            }

            return new JsonResult(new
            {
                products = products,
                totalPages = totalPages
            });
        }

        [BindProperty]
        public CreateProductDTO CreateProduct { get; set; } = new CreateProductDTO();

        // Create product
        public async Task<IActionResult> OnPostCreateAsync(IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Save the image to a specific path, e.g., wwwroot/images
                var adminfolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img/product");
                var userFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "../UserUI/wwwroot", "img/product");
                var fileName = Path.GetFileName(ImageFile.FileName);
                var adminFilePath = Path.Combine(adminfolderPath, fileName);
                var userFilePath = Path.Combine(userFolderPath, fileName);
                if (!System.IO.File.Exists(adminFilePath))
                {
                    Directory.CreateDirectory(adminfolderPath);
                    using (var stream = new FileStream(adminFilePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream); // Save the uploaded file
                    }
                }
                if (!System.IO.File.Exists(userFilePath))
                {
                    Directory.CreateDirectory(userFolderPath);
                    using (var stream = new FileStream(userFilePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream); // Save the uploaded file
                    }
                }

                CreateProduct.ImageURL = $"img/product/{fileName}"; // Set the image URL
            }
            else
            {
                CreateProduct.ImageURL = string.Empty; // Set default or empty if no image provided
            }
            var token = HttpContext.Session.GetString("token");
            Console.WriteLine($"Token: {token}");
            var createdProduct = await _productService.CreateProductAsync(CreateProduct, token);

            if (createdProduct == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create product.");
                return Page();
            }
            return RedirectToPage("ProductManagement");
        }

        // Delete product
        public async Task<IActionResult> OnGetDeleteAsync(int productId)
        {
            var token = HttpContext.Session.GetString("token");
            Console.WriteLine($"Token: {token}");
            try
            {
                var product = await _productService.GetProductByIdAsync(productId);

                var adminfolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var userFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "../UserUI/wwwroot");
                var adminFilePath = Path.Combine(adminfolderPath, product.ImageURL);
                var userFilePath = Path.Combine(userFolderPath, product.ImageURL);

                if (System.IO.File.Exists(adminFilePath))
                {
                    System.IO.File.Delete(adminFilePath); // Delete existing file if it exists
                }
                if (System.IO.File.Exists(userFilePath))
                {
                    System.IO.File.Delete(userFilePath); // Delete existing file if it exists
                }

                await _productService.DeleteProductAsync(productId, token);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Failed to delete product: {ex.Message}");
                return Page();
            }
        }

        //update product
        [BindProperty]
        public UpdateProductDTO UpdateProduct { get; set; } = new UpdateProductDTO();

        public async Task<IActionResult> OnPostUpdateAsync(IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Save the image to a specific path, e.g., wwwroot/images
                var adminfolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img/product");
                var userFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "../UserUI/wwwroot", "img/product");
                var fileName = Path.GetFileName(ImageFile.FileName);
                var adminFilePath = Path.Combine(adminfolderPath, fileName);
                var userFilePath = Path.Combine(userFolderPath, fileName);
                if (!System.IO.File.Exists(adminFilePath))
                {
                    System.IO.File.Delete(adminFilePath); // Delete existing file if it exists
                    Directory.CreateDirectory(adminfolderPath);
                    using (var stream = new FileStream(adminFilePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream); // Save the uploaded file
                    }
                }
                if (!System.IO.File.Exists(userFilePath))
                {
                    System.IO.File.Delete(userFilePath); // Delete existing file if it exists
                    Directory.CreateDirectory(userFolderPath);
                    using (var stream = new FileStream(userFilePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream); // Save the uploaded file
                    }
                }
                UpdateProduct.ImageURL = $"img/product/{fileName}"; // Set the image URL
            }
            else
            {
                var existingProduct = await _productService.GetProductByIdAsync(UpdateProduct.ProductID);
                if (existingProduct != null)
                {
                    UpdateProduct.ImageURL = existingProduct.ImageURL; // Retain the existing image URL if no new image is provided
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Product not found.");
                    return Page();
                }
            }
            var token = HttpContext.Session.GetString("token");
            Console.WriteLine($"Token: {token}");
            var updatedProduct = await _productService.UpdateProductAsync(UpdateProduct, token);
            if (updatedProduct == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update product.");
                return Page();
            }
            return RedirectToPage("ProductManagement");
        }

        public async Task<IActionResult> OnGetProductDetailAsync(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return new JsonResult(product);
        }

        public async Task<IActionResult> OnGetSearchProductsAsync(string searchTerm, int categoryId, bool status)
        {
            var product = await _productService.SearchProductsOdataAsync(searchTerm, categoryId, status);
            var totalPages = (int)Math.Ceiling(product.Count / 10f);

            return new JsonResult(new
            {
                products = product,
                totalPages = totalPages
            }); // Trả JSON danh sách sản phẩm theo danh mục
        }

        public async Task<ActionResult> OnGetProductsByCategoryAsyns(int categoryId)
        {
            var product = await _productService.GetProductsByCategoryAsync(categoryId);

            return new JsonResult(product); // Redirect to Index view with category products
        }
    }
}