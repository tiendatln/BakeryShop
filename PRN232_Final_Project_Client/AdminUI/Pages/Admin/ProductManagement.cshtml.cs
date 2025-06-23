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
            readProducts = (await _productService.GetAllProductsAsync()).ToList();
            if (readProducts == null)
            {
                ModelState.AddModelError(string.Empty, "No products found.");
            }
            else
            {
                // You can pass the products to the view if needed
                ViewData["Products"] = readProducts;
            }
        }

        public async Task<IActionResult> OnGetAllProductAsync([FromQuery]int page)
        {
            var allproducts = await _productService.GetAllProductsAsync();
            var totalPages = (int)Math.Ceiling(allproducts.Count / 10f);

            var skip = (page - 1) * 10; // Calculate the number of products to skip based on the current page

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

        public async Task<IActionResult> OnPostCreateAsync(IFormFile ImageFile)
        {


            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Save the image to a specific path, e.g., wwwroot/images
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img/product");
                Directory.CreateDirectory(folderPath);
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream); // Save the uploaded file
                }
                CreateProduct.ImageURL = $"img/product/{fileName}"; // Set the image URL
            }
            else
            {
                CreateProduct.ImageURL = string.Empty; // Set default or empty if no image provided
            }
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW4gVXNlciIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFkbWluQGV4YW1wbGUuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3NTA2ODk3MTMsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwMDkiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MTEyIn0.tSzzyW6C-f0BKgXbZf-R1wkNywVmhEJQeiK6rChflso";
            Console.WriteLine($"Token: {token}");
            var createdProduct = await _productService.CreateProductAsync(CreateProduct, token);

            if (createdProduct == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create product.");
                return Page();
            }
            return RedirectToPage("ProductManagement");
        }

        public async Task<IActionResult> OnGetDeleteAsync(int productId)
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW4gVXNlciIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFkbWluQGV4YW1wbGUuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3NTA2ODk3MTMsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwMDkiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MTEyIn0.tSzzyW6C-f0BKgXbZf-R1wkNywVmhEJQeiK6rChflso";
            Console.WriteLine($"Token: {token}");
            try
            {
                await _productService.DeleteProductAsync(productId, token);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Failed to delete product: {ex.Message}");
                return Page();
            }
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
    }
}
